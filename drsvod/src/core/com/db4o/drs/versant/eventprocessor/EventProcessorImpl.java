/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.eventprocessor;

import static com.db4o.qlin.QLinSupport.*;

import java.lang.reflect.*;
import java.util.*;

import com.db4o.drs.versant.*;
import com.db4o.drs.versant.ipc.*;
import com.db4o.drs.versant.ipc.tcp.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.drs.versant.metadata.ObjectInfo.Operations;
import com.db4o.drs.versant.metadata.ClassMetadata;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.versant.event.*;

public class EventProcessorImpl implements Runnable, EventProcessor {
	
	public static final String SIMPLE_NAME = ReflectPlatform.simpleName(EventProcessor.class);

	private static final long OBJECT_VERSION_FOR_PREEXISTING = 1; 	// can't use 0 because we query for > 0   

	private final VodEventClient _client;
	
	private volatile boolean _stopped;

	private final VodCobraFacade _cobra;
	
	private final TimeStampIdGenerator _timeStampIdGenerator = new TimeStampIdGenerator();
	
	private BlockingQueue4<Block4> _pausableTasks = new BlockingQueue<Block4>();
	
	private ServerChannelControl _incomingMessages;

	private final Object _lock = new Object();
	
	private final Map<String, List<ObjectInfo>> _objectInfos = new HashMap<String, List<ObjectInfo>>();
	
	private long _defaultSignatureLoid;
	
	private CommitTimestamp _commitTimestamp;

	private Map<String, Long> _knownClasses = new HashMap<String, Long>();

	private List<EventProcessorListener> _listeners = new ArrayList<EventProcessorListener>();

	private boolean _started;

	private final VodDatabase _vod;

	private long _commitTokenClassMetadataLoid;

	private long _barrierTokenClassMetadataLoid;
	
	private boolean _verbose;
	
	private Map<Long, List<Long>> _concurrentTimestamps = new HashMap<Long, List<Long>>();

	public EventProcessorImpl(VodEventClient client, VodDatabase vod, boolean verbose)  {
		_client = client;
		this._vod = vod;
	    _cobra = VodCobra.createInstance(vod);
	    produceLastTimestamp();
	    startChannelsFromKnownClasses();
	    defaultSignatureLoid();
	    System.out.println("VOD EventProcessor for dRS is listening for events.");
	    _verbose = verbose;
	}

	private void startChannelsFromKnownClasses() {
		Collection<Long> classMetadataLoids = _cobra.loids(ClassMetadata.class);
	    for (Long loid : classMetadataLoids) {
	    	ClassMetadata classMetadata = _cobra.objectByLoid(loid);
	    	createChannel(new ClassChannelSpec(classMetadata.name(), classMetadata.fullyQualifiedName(),  loid), false);
	    	_knownClasses.put(classMetadata.fullyQualifiedName(), classMetadata.loid());
		}
	}

	private void produceLastTimestamp() {
		_commitTimestamp = _cobra.singleInstanceOrDefault(CommitTimestamp.class, new CommitTimestamp(0));
		if(_commitTimestamp.value() == 0){
			_cobra.store(_commitTimestamp);
			_cobra.commit();
			println("No CommitTimestamp found. Initializing.");
			return;
			
		} 
		println("Timestamp read: " + _commitTimestamp.value());
		_timeStampIdGenerator.setMinimumNext(_commitTimestamp.value());
	}

	public void run() {
		_incomingMessages = TcpCommunicationNetwork.prepareCommunicationChannel(this, _vod, _client);
		startPausableTasksExecutor();
		synchronized (_listeners) {
			_started = true;
			listenerTrigger().ready();
		}
		try {
			_incomingMessages.join();
		} catch (InterruptedException e) {
		}
		shutdown();
	}
	

	private void startPausableTasksExecutor() {

		Thread t = new Thread("Pausable tasks executor") {
			@Override
			public void run() {
				runPausableTasks();
			}

		};
		t.setDaemon(true);
		t.start();
	}
	
	private void runPausableTasks() {
		try {
			Collection4<Block4> list = new Collection4<Block4>();
			while(!_stopped) {
				_pausableTasks.drainTo(list);
				synchronized (_lock) {
					if(_stopped){
						return;
					}
					Iterator4<Block4> it = list.iterator();
					while(it.moveNext()) {
						it.current().run();
					}
					list.clear();
				}
			}
		} catch (BlockingQueueStoppedException e){
		}
	}
	
	public void syncTimestamp(long newTimeStamp) {
		if(newTimeStamp > 0){
			_timeStampIdGenerator.setMinimumNext(newTimeStamp);
		}
	}
	
	public long lastTimestamp() {
		long timestamp = _timeStampIdGenerator.last();
		if(timestamp != 0){
			return timestamp;
		}
		return generateTimestamp();
	}
	
	public long generateTimestamp() {
		_timeStampIdGenerator.generate();
		return lastTimestamp();
	}
	
	public long beginReplicationGenerateTimestamp(){
		synchronized (_lock) {
			long timestamp = generateTimestamp();
			_concurrentTimestamps.put(timestamp, new ArrayList<Long>());
			return timestamp;
		}
	}
	
	public void replaceCommitTimestamp(long commitTimestamp, long syncedTimeStamp){
		synchronized (_lock) {
			List<Long> list = _concurrentTimestamps.get(commitTimestamp);
			_concurrentTimestamps.remove(commitTimestamp);
			for(List<Long> otherList : _concurrentTimestamps.values()){
				if(otherList.remove(commitTimestamp)){
					otherList.add(syncedTimeStamp);
				}
			}
			_concurrentTimestamps.put(syncedTimeStamp, list);
			syncTimestamp(syncedTimeStamp + 1);
		}
	}
	
	public long[] commitReplicationGetConcurrentTimestamps(long timestamp) {
		synchronized (_lock) {
			List<Long> list = _concurrentTimestamps.get(timestamp);
			_concurrentTimestamps.remove(timestamp);
			for(List<Long> otherList : _concurrentTimestamps.values()){
				otherList.add(timestamp);
			}
			for(Long runningTimestamp : _concurrentTimestamps.keySet()){
				list.add(runningTimestamp);
			}
			return Arrays4.toLongArray(list);
		}
	}
	
	private void shutdown() {
		_client.shutdown();
		synchronized (_lock) {
			_cobra.close();
		}
	}

	private void createChannel(final ClassChannelSpec channelSpec, boolean registerTransactionEvents) {
		EventChannel channel = _client.produceClassChannel(channelSpec._className, registerTransactionEvents);
		if(! channel.getListeners().isEmpty()){
			println("Listener already exists for " + channelSpec._className);
			return;
		}
		channel.addVersantEventListener (new ClassEventListener() {
			public void instanceModified (VersantEventObject event){
				queueObjectLifeCycleEvent(event, Operations.UPDATE, channelSpec);
			}
			public void instanceCreated (VersantEventObject event) {
				queueObjectLifeCycleEvent(event, Operations.CREATE, channelSpec);
			}
			public void instanceDeleted (VersantEventObject event) {
				queueObjectLifeCycleEvent(event, Operations.DELETE, channelSpec);				
			}
		});
		
		channel.addVersantEventListener (new TransactionMarkerEventListener() {
			public void endTransaction(final VersantEventObject event) {
				_pausableTasks.add(new TransactionCommitTask(event));
				
			}
			public void beginTransaction(VersantEventObject event) {
				// do nothing
			}
		});
		
		println("Listener channel created for class " + channelSpec._className);
	}
	
	private void queueObjectLifeCycleEvent(VersantEventObject event, Operations operation, ClassChannelSpec channelSpec) {
		_pausableTasks.add(new ObjectLifeCycleEventStoreTask(event.getTransactionID(), channelSpec._classMetadataLoid, event.getRaiserLoid(), operation));
	}

	private void println(String msg) {
		if(_verbose){
			System.out.println(msg);
		}
	}
	
	public void stop(){
		_stopped = true;
		_pausableTasks.stop();
		_incomingMessages.stop();
		try {
			_incomingMessages.join();
		} catch (InterruptedException e) {
//			e.printStackTrace();
		}
	}

	public static void unrecoverableExceptionOccurred(Throwable t) {
		t.printStackTrace();
		throw new RuntimeException(t);
		
		
    	// TODO: Now what???
    	// Events will be broken from now on. 
    	// Maybe store some kind of BigTrouble object in the database
    	// and react to it from some daemon code in the app?
	}
	
	private void commit(String transactionId) {
		synchronized (_lock) {
			List<ObjectInfo> infos = _objectInfos.remove(transactionId);
			if(infos == null){
				return;
			}
			for(ObjectInfo info: infos){
				if(info.classMetadataLoid() == commitTokenClassMetadataLoid()){
					return;
				}
				
				if(info.classMetadataLoid() == barrierTokenClassMetadataLoid()){
					listenerTrigger().onEvent(info.objectLoid(), info.version());
					return;
				}

			}
			for(ObjectInfo info: infos){
				long objectLoid = info.objectLoid();
				ObjectInfo objectInfo = prototype(ObjectInfo.class);
				ObjectInfo infoToStore = _cobra
					.from(ObjectInfo.class)
					.where(objectInfo.objectLoid())
					.equal(objectLoid)
					.singleOrDefault(info);
				if(infoToStore != info){
					infoToStore.copyStateFrom(info);
				}
				info.version(infoToStore.version());
				_cobra.store(infoToStore);
				println("stored: " + infoToStore);
			}
					
			_commitTimestamp.value(lastTimestamp());
			_cobra.store(_commitTimestamp);
			_cobra.commit();
			
			
			for(ObjectInfo info: infos){
				long objectLoid = info.objectLoid();
				long version = info.version();
				listenerTrigger().onEvent(objectLoid, version);
			}
			
			
			listenerTrigger().committed(transactionId);
			println(SIMPLE_NAME+" commit");
		}
		
	}

	private long commitTokenClassMetadataLoid() {
		ClassMetadata classMetadata = prototype(ClassMetadata.class);
		if(_commitTokenClassMetadataLoid > 0) {
			return _commitTokenClassMetadataLoid;
		}
		ClassMetadata storedClassMetadata = 
			_cobra.from(ClassMetadata.class)
			.where(classMetadata.fullyQualifiedName())
			.equal(ReplicationCommitToken.class.getName()).singleOrDefault(null);
		if(storedClassMetadata != null){
			_commitTokenClassMetadataLoid = storedClassMetadata.loid();
		}
		return _commitTokenClassMetadataLoid;
	}
	
	private long barrierTokenClassMetadataLoid() {
		ClassMetadata classMetadata = prototype(ClassMetadata.class);
		if(_barrierTokenClassMetadataLoid > 0) {
			return _barrierTokenClassMetadataLoid;
		}
		ClassMetadata storedClassMetadata = 
			_cobra.from(ClassMetadata.class)
			.where(classMetadata.fullyQualifiedName())
			.equal(BarrierCommitToken.class.getName()).singleOrDefault(null);
		if(storedClassMetadata != null){
			_barrierTokenClassMetadataLoid = storedClassMetadata.loid();
		}
		return _barrierTokenClassMetadataLoid;
	}

	private final class TransactionCommitTask implements Block4 {
		private final VersantEventObject _event;

		private TransactionCommitTask(VersantEventObject event) {
			_event = event;
		}

		public void run() {
			commit(_event.getTransactionID());
		}

		@Override
		public String toString() {
			return "pausable commit";
		}
	}


	public class ClassChannelSpec {

		public final String _className;
		
		public final String _fullyQualifiedName;
		
		public final long _classMetadataLoid;

		public ClassChannelSpec(String className, String fullyQualifiedName, long classMetadataLoid) {
			_className = className;
			_fullyQualifiedName = fullyQualifiedName;
			_classMetadataLoid = classMetadataLoid;
		}
		
		@Override
		public String toString() {
			return com.db4o.internal.Reflection4.dump(this);
		}

	}
	
	private class ObjectLifeCycleEventStoreTask implements Block4 {

		private final String _transactionId;
		private final long _classLoid;
		private final String _objectLoid;
		private final Operations _operation;
		private final long _timeStamp;
		
		public ObjectLifeCycleEventStoreTask(String transactionId, long classLoid, String objectLoid, Operations operation) {
			_transactionId = transactionId;
			_classLoid = classLoid;
			_objectLoid = objectLoid;
			_operation = operation;
			_timeStamp = generateTimestamp();
		}
		
		public void run() {
			
			long loid = VodCobra.loidAsLong(_objectLoid);
			
			ObjectInfo objectInfo = 
				new ObjectInfo(
						defaultSignatureLoid(),
						_classLoid,
						loid,
						_timeStamp,
						_timeStamp,
						_operation.value,
						0);
			List<ObjectInfo> infos = _objectInfos.get(_transactionId);
			if(infos == null){
				infos = new java.util.LinkedList<ObjectInfo>();
				_objectInfos.put(_transactionId, infos);
			}
			infos.add(objectInfo);
			println("Event registered: " + objectInfo);
		}

		@Override
		public String toString() {
			return getClass().getSimpleName()+ ": " + _classLoid + ", " + _objectLoid + ", " + _operation;
		}
	}
	
	public long defaultSignatureLoid() {
		if (_defaultSignatureLoid > 0) {
			return _defaultSignatureLoid;
		}
		_defaultSignatureLoid = _cobra.queryForMySignatureLoid();
		if (_defaultSignatureLoid != 0) {
			return _defaultSignatureLoid;
		}
		DatabaseSignature databaseSignature = new DatabaseSignature(_cobra.signatureBytes(_cobra.databaseId()));
		// TODO: There may be a race condition if the replication
		// provider creates the same signature at the same time.
	
		_cobra.store(databaseSignature);
		_cobra.commit();
		_defaultSignatureLoid = databaseSignature.loid();
		return _defaultSignatureLoid;
	}

	public void addListener(EventProcessorListener l) {
		synchronized (_listeners) {
			_listeners.add(l);
			if (_started) {
				l.ready();
			}
		}
	}
	
	public void removeListener(EventProcessorListener listener) {
		synchronized (_listeners) {
			_listeners.remove(listener);
		}
	}
	
	private EventProcessorListener listenerTrigger() {
		return (EventProcessorListener) Proxy.newProxyInstance(getClass().getClassLoader(), new Class<?>[]{EventProcessorListener.class}, new InvocationHandler() {
			
			public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
				ArrayList<EventProcessorListener> ls;
				synchronized (_listeners) {
					ls = new ArrayList<EventProcessorListener>(_listeners);
				}
				for(EventProcessorListener l : ls) {
					try {
						method.invoke(l, args);
					} catch (InvocationTargetException e) {
						throw e.getCause();
					}
				}
				return null;
			}
		});
	}

	public Map<String, Long> ensureMonitoringEventsOn(final String className) {

		Map<String, Long> classIds = new HashMap<String, Long>();

		synchronized (_lock) {
			if(className == null){
				return classIds;
			}
			
			Long classMetadataLoid = _knownClasses.get(className);
			if (classMetadataLoid == null) {
				classMetadataLoid = produceChannel(className);
				_knownClasses.put(className, classMetadataLoid);
				classIds.put(className, classMetadataLoid);
				createObjectInfosForPreexistingObjects(className, classMetadataLoid);
			} else {
				classIds.put(className, classMetadataLoid);
			}
		}

		return classIds;
	}

	private void createObjectInfosForPreexistingObjects(final String className, long classMetadataLoid) {
		long[] loids = _cobra.loidsForStoredObjectsOfClass(className);
		for (int i = 0; i < loids.length; i++) {
			ObjectInfo objectInfo = new ObjectInfo(
					_defaultSignatureLoid, 
					classMetadataLoid, 
					loids[i],
					_timeStampIdGenerator.generate(),
					OBJECT_VERSION_FOR_PREEXISTING,  // this looks broken, pre-existing objects would not be found.
					Operations.CREATE.value,
					0);
			_cobra.store(objectInfo);
		}
		_cobra.commit();
	}

	private long produceChannel(String fullyQualifiedName) {
		
		Long cmLoid = _knownClasses.get(fullyQualifiedName);
		
		if (cmLoid != null) {
			return cmLoid;
		}
		
		String schemaName = schemaFor(fullyQualifiedName);
		
		ClassMetadata cm = ensureClassMetadata(fullyQualifiedName, schemaName);

		createChannel(new ClassChannelSpec(schemaName, fullyQualifiedName, cm.loid()), false);
		
		_knownClasses.put(fullyQualifiedName, cm.loid());
		
		return cm.loid();
	}

	private ClassMetadata ensureClassMetadata(String fullyQualifiedName, String schemaName) {
		ClassMetadata classMetadata = prototype(ClassMetadata.class);
		ClassMetadata storedClassMetadata = _cobra.from(ClassMetadata.class).where(classMetadata.name()).equal(schemaName).singleOrDefault(null);
		if(storedClassMetadata != null){
			return storedClassMetadata;
		}
		classMetadata = new ClassMetadata(schemaName, fullyQualifiedName);
		_cobra.store(classMetadata);
		return classMetadata;
	}

	private String schemaFor(String fullyQualifiedName) {
		return _cobra.schemaName(fullyQualifiedName);
	}

	

}
