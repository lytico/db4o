/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import static com.db4o.drs.foundation.Logger4Support.*;
import static com.db4o.qlin.QLinSupport.*;

import java.util.*;

import javax.jdo.*;
import javax.jdo.spi.*;

import com.db4o.*;
import com.db4o.drs.foundation.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.versant.VodJdo.ObjectCommittedListener;
import com.db4o.drs.versant.ipc.*;
import com.db4o.drs.versant.ipc.EventProcessor.EventProcessorListener;
import com.db4o.drs.versant.ipc.tcp.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.drs.versant.metadata.ObjectInfo.Operations;
import com.db4o.foundation.*;

public class VodReplicationProvider implements TestableReplicationProviderInside, LoidProvider {
	
	private final VodDatabase _vod;
	
	private final VodCobraFacade _cobra;
	
	private final VodJdoFacade _jdo;
	
	private GenericObjectReferenceMap<ReplicationReferenceImpl> _replicationReferences = new GenericObjectReferenceMap<ReplicationReferenceImpl>();
	
	private final Signatures _signatures = new Signatures();
	
	private final Map<String, Long> _knownClasses = new HashMap<String, Long>();

	private ReplicationCommitRecord _replicationCommitRecord;
	
	private final ReadonlyReplicationProviderSignature _mySignature;
	
	private final long _mySignatureLoid;
	
	private volatile long _commitTimestamp;
	
	List<Long> _ignoreEventsForLoid = new java.util.LinkedList<Long>();
	
	private final Map<Class, List<Class>> _classHierarchy = new HashMap<Class, List<Class>>();
	
	private volatile ClientChannelControl _control;

	public VodReplicationProvider(VodDatabase vod) {
		_vod = vod;
		_cobra = VodCobra.createInstance(vod);
		_jdo = VodJdo.createInstance(vod);
		loadSignatures();
		loadKnownClasses();
		
		final byte[] signatureBytes = _cobra.signatureBytes(_cobra.databaseId());
		_mySignatureLoid = produceSignature(new Signature(signatureBytes));
		
		_jdo.commit();
		prepareJdoListener();
		_mySignature = new ReadonlyReplicationProviderSignature() {
			public byte[] getSignature() {
				return signatureBytes;
			}
			public long getId() {
				return _mySignatureLoid;
			}
			public long getCreated() {
				return _mySignatureLoid;
			}
		};
		
	}
	
	
	private static final Class[] IGNORED_CLASSES = {ObjectInfo.class};

	private void prepareJdoListener() {
		_jdo.addObjectCommittedListener(new ObjectCommittedListener() {
			public void committed(Object object) {
				Class clazz = object.getClass();
				for (int i = 0; i < IGNORED_CLASSES.length; i++) {
					if(clazz == IGNORED_CLASSES[i]){
						return;
					}
				}
				Long classMetadataLoid = _knownClasses.get(clazz.getName());
				if(classMetadataLoid == null) {
					 classMetadataLoid = ensureClassKnown(clazz);
				}
			}
		});
	}
	
	public long ensureClassKnown(Class clazz) {
		ensureClassHierarchyKnown(clazz);
		String className = clazz.getName();
		Long loid = _knownClasses.get(className);
		if (loid != null) {
			return loid;
		}
		
		String schemaName = _cobra.schemaName(clazz.getName());
		if(schemaName == null){
			// This is expected for built-in Java classes
			// like List. 
			return 0;
		}
		ClassMetadata cm = new ClassMetadata(schemaName, className);
		
		_cobra.store(cm);
		log("Classmetadata stored " + className);
		_cobra.commit();
		loid = cm.loid();
		_knownClasses.put(className, loid);
		syncEventProcessor().ensureMonitoringEventsOn(className);
		return loid; 
	}
	
	private void loadKnownClasses() {
		for (ClassMetadata classMetadata : _cobra.query(ClassMetadata.class)) {
			_knownClasses.put(classMetadata.fullyQualifiedName(), classMetadata.loid());
			try {
				ensureClassHierarchyKnown(Class.forName(classMetadata.fullyQualifiedName()));
			} catch (ClassNotFoundException e) {
				e.printStackTrace();
			}
		}
	}
	
	private void ensureClassHierarchyKnown(Class clazz) {
		if(clazz == Object.class){
			return;
		}
		if (_classHierarchy.containsKey(clazz)){
			return;
		}
		addToHierarchy(clazz);
	}

	private void addToHierarchy(Class clazz) {
		Class superclass = clazz.getSuperclass();
		ensureClassHierarchyKnown(superclass);
		if(superclass != Object.class){
			List<Class> children = _classHierarchy.get(superclass);
			children.add(clazz);
		}
		_classHierarchy.put(clazz, new ArrayList<Class>());
	}

	private void loadSignatures() {
		for (DatabaseSignature entry : _cobra.query(DatabaseSignature.class)) {
			if(DrsDebug.verbose){
				System.out.println(entry);
			}
			_signatures.add(new Signature(entry.signature()), entry.loid());
		}
	}
	
	
	public void commit() {
		_jdo.commit();
	}

	public void delete(Object obj) {
		_jdo.delete(obj);
	}

	public void deleteAllInstances(Class clazz) {
		if(!_cobra.isKnownClass(clazz)) {
			return;
		}
		_jdo.deleteAll(clazz);
	}

	public ObjectSet getStoredObjects(Class clazz) {
		return new ObjectSetCollectionFacade(_jdo.query(clazz));
	}

	public void storeNew(Object obj) {
		logIdentity(obj, getName());
		if(obj == null){
			throw new IllegalArgumentException();
		}
		if(! (obj instanceof PersistenceCapable)){
			String msg = "Object of " + obj.getClass() + " does not implement PersistenceCapable. Recommended action: Enhance all persistent classes for JDO using an enhancer.";
			throw new IllegalStateException(msg);
		}
		ensureClassKnown(obj.getClass());
		_jdo.store(obj);
	}

	public void update(Object obj) {
		// do nothing, Transparent Persistence
	}

	public void destroy() {
		_jdo.close();
		_cobra.close();
		control().stop();
		try {
			control().join();
		} catch (InterruptedException e) {
		}
	}

	public void activate(Object object) {
		// do nothing
		// JDO is transparent activation
	}

	public boolean supportsHybridCollection() {
		return false;
	}

	public boolean supportsMultiDimensionalArrays() {
		return true;
	}

	public boolean supportsRollback() {
		return true;
	}

	public void clearAllReferences() {
		_replicationReferences.clear();
	}

	public void commitReplicationTransaction() {
		updateReplicationCommitToken();
		_jdo.commit();
		storeReplicationCommitRecord();
		_replicationReferences = new GenericObjectReferenceMap<ReplicationReferenceImpl>();
	}

	private void updateReplicationCommitToken() {
		ensureClassKnown(ReplicationCommitToken.class);
		Collection<ReplicationCommitToken> commitTokens = _jdo.query(ReplicationCommitToken.class);
		if(commitTokens.isEmpty()){
			_jdo.store(new ReplicationCommitToken());
			return;
		}
		ReplicationCommitToken commitToken = commitTokens.iterator().next();
		commitToken.incrementCounter();
	}
	
	private long updateBarrierCommitToken() {
		ensureClassKnown(BarrierCommitToken.class);
		Collection<BarrierCommitToken> commitTokens = _jdo.query(BarrierCommitToken.class);
		if(commitTokens.isEmpty()){
			BarrierCommitToken barrierCommitToken = new BarrierCommitToken();
			_jdo.store(barrierCommitToken);
			return _jdo.loid(barrierCommitToken);
		}
		BarrierCommitToken commitToken = commitTokens.iterator().next();
		commitToken.incrementCounter();
		return _jdo.loid(commitToken);
	}
	
	public long getLastReplicationVersion() {
		ensureReplicationSessionActive();
		return _replicationCommitRecord.timestamp();
	}

	public String getName() {
		return _vod.name();
	}

	public ReadonlyReplicationProviderSignature getSignature() {
		return _mySignature;
	}

	public ReplicationReference produceReference(final Object obj, Object referencingObj, String fieldName) {
		ensureClassKnown(obj.getClass());
		ReplicationReferenceImpl reference = _replicationReferences.get(obj);
		if (reference != null){
			return reference;
		}
		reference = produceNewReference(obj);
		if(reference == null) {
			return null;
		}
		_replicationReferences.put(reference);
		return reference; 
	}
	
	private long loidFrom(DrsUUID uuid) {
		Signature signature = new Signature(uuid.getSignaturePart()); 
		ObjectInfo objectInfo = prototype(ObjectInfo.class);
		ObjectSet<ObjectInfo> infos = _cobra.from(ObjectInfo.class)
			.where(objectInfo.uuidLongPart()).equal(uuid.getLongPart()).select();
		for (ObjectInfo info : infos) {
			if(signature.equals(_signatures.signatureForLoid(info.signatureLoid()))){
				return info.objectLoid();
			}
		}
		return 0;
	}

	public ReplicationReference referenceNewObject(Object obj,
			ReplicationReference counterpartReference,
			ReplicationReference referencingObjRef, String fieldName) {
		
		DrsUUID uuid = counterpartReference.uuid();
		
		long version = counterpartReference.version();
		
		ReplicationReferenceImpl ref = new ReplicationReferenceImpl(obj, uuid, version);
		_replicationReferences.put(ref);
		return ref;
	}

	public void replicateDeletion(DrsUUID uuid) {
		Object object = _jdo.objectByLoid(loidFrom(uuid));
		if (object == null) {
			return;
		}
		_jdo.delete(object);
	}

	public void rollbackReplication() {
		clearAllReferences();
		_jdo.rollback();
	}
	
	public void startReplicationTransaction(ReadonlyReplicationProviderSignature peer) {
		clearAllReferences();
		long peerId = produceSignature(new Signature(peer.getSignature()));
		long lowerId = Math.min(peerId, _mySignatureLoid);
		long higherId = Math.max(peerId, _mySignatureLoid);
		
		_replicationCommitRecord = commitRecordFor(lowerId, higherId);
		
		waitForPreviousCommits();
		
		if(_replicationCommitRecord == null){
			_replicationCommitRecord = new ReplicationCommitRecord(databaseSignature(lowerId), databaseSignature(higherId));
			_replicationCommitRecord.concurrentTimestamps(new long[]{});
			_cobra.store(_replicationCommitRecord);
			_cobra.commit();
		} else {
			syncEventProcessor().syncTimestamp(_replicationCommitRecord.timestamp() + 1);
			if(_replicationCommitRecord.concurrentTimestamps() == null){
				_replicationCommitRecord.concurrentTimestamps(new long[]{});
			}
		}
		_commitTimestamp = syncEventProcessor().beginReplicationGenerateTimestamp();
	}

	public void waitForPreviousCommits() {
		final long barrierLoid = updateBarrierCommitToken();
		final BlockingQueue<Long> barrierQueue = new BlockingQueue<Long>();
		
		EventProcessorListener listener = new AbstractEventProcessorListener() {
			@Override
			public void onEvent(long loid, long version) {
				if(loid == barrierLoid){
					barrierQueue.add(loid);
				}
			}
		};
		syncEventProcessor().addListener(listener);
		
		_jdo.commit();
		
		barrierQueue.next();
		
		syncEventProcessor().removeListener(listener);
	}

	private ReplicationCommitRecord commitRecordFor(long lowerId, long higherId) {
		Collection<ReplicationCommitRecord> q = _cobra.query(ReplicationCommitRecord.class);
		for (ReplicationCommitRecord r : q) {
			if (r.lowerPeer().loid() == lowerId && r.higherPeer().loid() == higherId) {
				return r;
			}
		}
		return null;
	}

	private DatabaseSignature databaseSignature(long signatureLoid) {
		return _cobra.objectByLoid(signatureLoid);
	}
	
	public void storeReplica(Object obj){
		if (shouldIgnore(obj)) {
			return;
		}
		
		logIdentity(obj, getName());
		
		if (!(obj instanceof PersistenceCapable)) {
			throw new IllegalArgumentException(VodReplicationProvider.class.getSimpleName()+" can only handle " + PersistenceCapable.class.getSimpleName() + " objects");
		}
		
		ReplicationReferenceImpl ref = _replicationReferences.get(obj);
		if (ref == null) {
			throw new RuntimeException("Reference should always be available before storeReplica");
		}
		long classMetadataLoid = ensureClassKnown(obj.getClass());
		long loid = _jdo.loid(obj);
		
		boolean isNew = loid == 0;
		
		ObjectInfo objectInfo = null;
		
		if(isNew){
			_jdo.store(obj);
			loid = _jdo.loid(obj);
		} else {
			objectInfo = objectInfoForLoid(loid);
			isNew = objectInfo == null; 
		}

		if (isNew) {
			Signature signature = new Signature(ref.uuid().getSignaturePart());
			long signatureLoid = produceSignature(signature);
			long otherLongPart = ref.uuid().getLongPart();
			objectInfo = new ObjectInfo(signatureLoid, classMetadataLoid, loid,  otherLongPart, _commitTimestamp, Operations.CREATE.value, 0);
		} else{
			objectInfo.version(_commitTimestamp);
			objectInfo.operation(Operations.UPDATE.value);
		}
		
		_cobra.store(objectInfo);
		_cobra.commit();
		
		logIdentity(obj, String.valueOf(loid));
	}

	private ObjectInfo objectInfoForLoid(long objectLoid) {
		ObjectInfo objectInfo = prototype(ObjectInfo.class);
		return _cobra.from(ObjectInfo.class).where(objectInfo.objectLoid()).equal(objectLoid).singleOrDefault(null);
	}

	private void storeReplicationCommitRecord() {
		_replicationCommitRecord.timestamp(_commitTimestamp);
		long[] concurrentTimestamps = syncEventProcessor().commitReplicationGetConcurrentTimestamps(_commitTimestamp);
		_replicationCommitRecord.concurrentTimestamps(concurrentTimestamps);
		_cobra.store(_replicationCommitRecord);
		_cobra.commit();
	}

	public void visitCachedReferences(Visitor4 visitor) {
		_replicationReferences.visitEntries(visitor);
	}

	public boolean wasModifiedSinceLastReplication(ReplicationReference reference) {
		long timestamp = reference.version();
		if (timestamp > getLastReplicationVersion()){
			return true;
		}
		long[] concurrentTimestamps = _replicationCommitRecord.concurrentTimestamps();
		for (int i = 0; i < concurrentTimestamps.length; i++) {
			if(concurrentTimestamps[i] == timestamp){
				return true;
			}
		}
		return false;
	}

	public ObjectSet objectsChangedSinceLastReplication() {
		return queryForModifiedObjects(null);
	}
	
	public ObjectSet objectsChangedSinceLastReplication(Class clazz) {
		return queryForModifiedObjects(clazz);
	}

	private void ensureReplicationSessionActive() {
		if(_replicationCommitRecord == null){
			throw new IllegalStateException("No active replication session. Call Replication.begin() to start a replication session.");
		}
	}

	private ObjectSet queryForModifiedObjects(Class clazz) {
		String filter = "((this.version > " + getLastReplicationVersion() + " && this.version < " + _commitTimestamp + ")";
		long[] concurrentTimestamps = _replicationCommitRecord.concurrentTimestamps();
		if(concurrentTimestamps != null){
			for (int i = 0; i < concurrentTimestamps.length; i++) {
				filter += " || this.version == " + concurrentTimestamps[i];  
			}
		}
		filter += ")";
		if(clazz != null){
			ensureClassKnown(clazz);
			filter += " && (" + classMetadataLoidFilter(clazz) + ")";
		}
		Set<Long> loids = new HashSet<Long>();
		Collection<ObjectInfo> infos = _jdo.query(ObjectInfo.class, filter);
		for (ObjectInfo info : infos) {
			if(Operations.forValue(info.operation()) != Operations.DELETE){
				loids.add(info.objectLoid());
			}
		}
		Collection<Object> objects = new ArrayList<Object>(loids.size());
		for (Long loid : loids) {
			try{
				objects.add(_jdo.objectByLoid(loid));
			} catch(JDOObjectNotFoundException ex){
				// This may happen, either because the JDO transaction is not
				// yet committed or deletion has happened and the EventProcessor
				// has not followed up.
				
				// FIXME: Log here
			}
		}
		return new ObjectSetCollectionFacade(objects);
	}

	private String classMetadataLoidFilter(Class clazz) {
		String filter = "this.classMetadataLoid == " + _knownClasses.get(clazz.getName());
		List<Class> children = _classHierarchy.get(clazz);
		if(children != null){
			for (Class childClass : children) {
				filter += " || " + classMetadataLoidFilter(childClass);
			}
		}
		return filter;
	}
	
	public void debug(){
		// useful debug code left here, 
		// just set the class to print all ObjectInfos for a class
		Class clazz = null;
		logObjectInfoJdo(clazz);
		logObjectInfoCobra(clazz);
	}
	
	private void logObjectInfoJdo(Class clazz) {
		System.err.println("JDO");
		String filter = classMetadataLoidFilter(clazz);
		Collection<ObjectInfo> infos = _jdo.query(ObjectInfo.class, filter);
		for (ObjectInfo info : infos) {
			if(Operations.forValue(info.operation()) != Operations.DELETE){
				System.err.println(info);
			}
		}
	}
	
	private void logObjectInfoCobra(Class clazz) {
		System.err.println("Cobra");
		ObjectInfo objectInfo = prototype(ObjectInfo.class);
		ObjectSet<ObjectInfo> infos = _cobra.from(ObjectInfo.class).where(objectInfo.classMetadataLoid()).equal(_knownClasses.get(clazz.getName())).select();
		for (ObjectInfo info : infos) {
			if(Operations.forValue(info.operation()) != Operations.DELETE){
				System.err.println(info);
			}
		}
	}

	public void replicationReflector(ReplicationReflector replicationReflector) {
	}

	public boolean isProviderSpecific(Object original) {
		return original.getClass().getName().startsWith("com.versant.");
	}
	
	public ReplicationReference produceReference(Object obj) {
		return produceReference(obj, null, null);
	}
	
	private ReplicationReferenceImpl produceNewReference(final Object obj) {
		if(obj == null){
			throw new IllegalArgumentException();
		}
		if (shouldIgnore(obj)) {
			return null;
		}
		long loid = _jdo.loid(obj);
		if(loid == 0) {
			return null;
		}
		ObjectInfo objectInfo = prototype(ObjectInfo.class);
		ObjectInfo info = 
			_cobra.from(ObjectInfo.class)
				.where(objectInfo.objectLoid())
				.equal(loid)
				.singleOrDefault(null);
		if(DrsDebug.verbose){
			if(info != null){
				System.out.println("#creationVersion() found: " + info);
			} else {
				System.out.println("No Objectinfo found for loid: " + loid);
			}
		}
		if(info == null){
			return null;
		}
		Signature signature = _signatures.signatureForLoid(info.signatureLoid());
		if(signature == null){
			throw new IllegalStateException("signature not expected to be null ");
		}
		DrsUUIDImpl uuid = new DrsUUIDImpl(signature, info.uuidLongPart());
		return new ReplicationReferenceImpl(obj, uuid, info.version());
	}

	protected boolean shouldIgnore(Class aClass) {
		return aClass != null && aClass.isEnum();
	}

	protected boolean shouldIgnore(Object obj) {
		return obj.getClass().isEnum();
	}

	public ReplicationReferenceImpl produceReferenceByUUID(DrsUUID uuid, Class hint) {
		if(uuid == null){
			throw new IllegalArgumentException();
		}
		if (shouldIgnore(hint)) {
			return null;
		}
		ReplicationReferenceImpl reference = _replicationReferences.getByUUID(uuid);
		if(reference != null){
			return reference;
		}
		ObjectInfo objectInfo = prototype(ObjectInfo.class);
		ObjectSet<ObjectInfo> infos = _cobra
			.from(ObjectInfo.class)
			.where(objectInfo.uuidLongPart())
			.equal(uuid.getLongPart())
			.select();
		if(infos.size() == 0){
			return null;
		}
		
		Long signatureLoid = _signatures.loidForSignature(new Signature(uuid.getSignaturePart()));
		if(signatureLoid == null){
			return null;
		}
		
		ObjectInfo foundInfo = null;
		for (ObjectInfo info : infos) {
			if(info.signatureLoid() == signatureLoid){
				foundInfo = info;
				break;
			}
		}
		
		if(foundInfo == null || foundInfo.operation() == Operations.DELETE.value){
			return null;	
		}
		
		reference = produceNewReference(_jdo.objectByLoid(foundInfo.objectLoid()));
		_replicationReferences.put(reference);
		return reference; 
	}
	
	private long produceSignature(Signature signature) {
		Long loid = _signatures.loidForSignature(signature);
		if(loid != null){
			return loid;
		}
		long signatureLoid = storeSignature(signature);
		_signatures.add(signature, signatureLoid);
		return signatureLoid;
	}
	
	private long storeSignature(Signature signature) {
		DatabaseSignature databaseSignature = new DatabaseSignature(signature.bytes);
		_cobra.store(databaseSignature);
		_cobra.commit();
		return databaseSignature.loid();
	}

	public long loid(Object obj) {
		return _jdo.loid(obj);
	}

	public EventProcessor syncEventProcessor() {
		return control().sync();
	}
	
	@Override
	public String toString() {
		return getName();
	}

	public Object replaceIfSpecific(Object value) {
		if (value instanceof com.versant.core.jdo.sco.Date) {
			return new Date(((com.versant.core.jdo.sco.Date) value).getTime());
		}
		return value;
	}

	public boolean isSecondClassObject(Object obj) {
		return obj instanceof com.versant.core.jdo.sco.Date;
	}

	/**
	 * ensures that the EventProcessor is listening for changes
	 * on the specified classes.
	 */
	public void listenForReplicationEvents(Class...classes) {
		for (Class clazz : classes) {
			ensureClassKnown(clazz);
		}
	}

	public long objectVersion(Object object) {
		ObjectInfo objInfo = objectInfoForLoid(loid(object));
		return objInfo == null ? 0 : objInfo.version();
	}
	
	public long creationTime(Object object) {
		ObjectInfo objInfo = objectInfoForLoid(loid(object));
		return objInfo == null ? 0 : objInfo.uuidLongPart();
	}


	public void ensureVersionsAreGenerated() {
	}

	public TimeStamps timeStamps() {
		return new TimeStamps(_replicationCommitRecord.timestamp(), _commitTimestamp);
	}

	public void syncCommitTimestamp(long syncedTimeStamp) {
		if(syncedTimeStamp <= _commitTimestamp){
			return;
		}
		syncEventProcessor().replaceCommitTimestamp(_commitTimestamp, syncedTimeStamp);
		_commitTimestamp = syncedTimeStamp;
	}

	private ClientChannelControl control() {
		if (_control == null) {
			synchronized (this) {
				if (_control == null) {
					_control = TcpCommunicationNetwork.newClient(_vod);
				}
			}
		}
		return _control;
	}
}
