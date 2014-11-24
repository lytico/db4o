/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import static com.db4o.qlin.QLinSupport.*;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.foundation.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.versant.cobra.qlin.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.drs.versant.metadata.ClassMetadata;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.encoding.*;
import com.db4o.qlin.*;
import com.versant.odbms.*;
import com.versant.odbms.model.*;
import com.versant.odbms.query.*;

public class VodCobra implements QLinable, VodCobraFacade{
	
	private static final long LOCK_TIMEOUT = 30000;
	
	private static final int WAIT_BEFORE_LOCK_RETRY = 10;
	
	private static final String CLASS_NAME_FIELD_NAME = "name";

	private static final String CLASS_CLASS_NAME = "class";

	private static final long INVALID_LOID = 0L;
	
	public static final long INVALID_TIMESTAMP = 0L;
	
	private final VodDatabase _vod;
	
	private DatastoreManager _dm;
	
	private Set<String> _storedClassNames;
	
	private long _defaultSignatureLoid;
	
	private Map<Class, ClassMetadata> _classToClassMetadata = new HashMap<Class, ClassMetadata>();
	
	public static VodCobraFacade createInstance(VodDatabase vod) {
		return new VodCobra(vod);
		// return ProxyUtil.throwOnConcurrentAccess(new VodCobra(vod));
	}

	private VodCobra(VodDatabase vod) {
		_vod = vod;
		_dm = vod.createDatastoreManager();
		_dm.beginTransaction();
	}
	
	private DatastoreManagerFactory datastoreManagerFactory(){
		return _vod.datastoreManagerFactory();
	}
	
	public static long loidAsLong(String loidAsString){
		return DatastoreLoid.asValue(loidAsString);
	}
	
	public static String loidAsString(long loid){
		return DatastoreLoid.asString(loid);
	}
	
	public void close() {
		_dm.rollbackTransaction();
		_dm.close();
	}

	public long uuidLongPart(long loid) {
		ObjectInfo objectInfo = prototype(ObjectInfo.class);
		ObjectInfo storedInfo = 
			from(ObjectInfo.class)
				.where(objectInfo.objectLoid())
				.equal(loid)
				.singleOrDefault(null);
		if(DrsDebug.verbose){
			System.out.println("#creationVersion() found: " + storedInfo);
		}
		if(storedInfo == null){
			return INVALID_TIMESTAMP;
		}
		return storedInfo.uuidLongPart();
	}
	
	public void produceSchema(Class<?> clazz){
		UserSchemaModel userSchemaModel = VodCobraSchemaManager.buildUserSchemaModel(clazz);
		VodCobraSchemaManager.defineSchema(datastoreManagerFactory(), userSchemaModel);
	}

	public long store(Object obj) {
		if(obj instanceof VodLoidAwareObject){
			long loid = ((VodLoidAwareObject)obj).loid();
			if(loid > 0){
				store(loid, obj);
				return loid;
			}
		}
		DatastoreObject datastoreObject = newDatastoreObject(obj.getClass());
		writeFields(obj, datastoreObject);
		write(datastoreObject);
		long loid = datastoreObject.getLOID();
		if(obj instanceof VodLoidAwareObject){
			((VodLoidAwareObject)obj).loid(loid);
		}
		return loid;
	}

	public void create(long loid, Object obj) {
		if(DrsDebug.verbose) {
			System.out.println(String.format("Created loid: %d (%x) for object of type %s" , loid, loid, obj.getClass().getName()));
		}
		DatastoreInfo info = datastoreInfo();
		SchemaEditor editor = _dm.getSchemaEditor();

		DatastoreSchemaClass dsc = editor.findClass(obj.getClass().getName(), info);
		
		DatastoreObject datastoreObject = new DatastoreObject(loid, dsc, info);
		datastoreObject.allocate();
		datastoreObject.setIsNew(true);
		
		write(datastoreObject);
		
		if (obj instanceof VodLoidAwareObject) {
			((VodLoidAwareObject)obj).loid(loid);
		}
	}

	private DatastoreInfo datastoreInfo() {
		return _dm.getPrimaryDatastoreInfo();
	}

	private void writeFields(Object obj, DatastoreObject datastoreObject) {
		for (CobraField field : fields(classOf(datastoreObject))) {
			field.write(obj, datastoreObject);
		}
	}

	private void write(DatastoreObject datastoreObject) {
		_dm.groupWriteObjects(new DatastoreObject[] { datastoreObject }, Options.NO_OPTIONS);
	}
	
	public void store(long loid, Object obj) {
		DatastoreObject datastoreObject = datastoreObjectForUpdate(loid);
		Class<Object> clazz = classOf(datastoreObject);
		for (CobraField field : fields(clazz)) {
			field.write(obj, datastoreObject);
		}
		write(datastoreObject);
	}
	
	public Collection<Long> loids(Class<?> extent) {
		Object[] loids = datastoreLoids(extent);
		ArrayList<Long> result = new ArrayList<Long>();
		for ( int i = 0; i < loids.length; i++ ){
			result.add(((DatastoreLoid) loids[i]).value());
		}
		return result;
	}
	
	private <T> Class<T> classOf(DatastoreObject datastoreObject) {
		DatastoreSchemaClass schemaClass = datastoreObject.getSchemaClass();
		Class<T> clazz;
		try {
			clazz = (Class<T>) Class.forName(schemaClass.getName());
		} catch (ClassNotFoundException e) {
			throw new RuntimeException(e);
		}
		return clazz;
	}
	
	private DatastoreObject newDatastoreObject(Class clazz) {
		DatastoreObject datastoreObject = new DatastoreObject(_dm.getNewLoid(datastoreInfo()), datastoreSchemaClass(clazz), datastoreInfo());
		datastoreObject.setTimestamp(1);
		datastoreObject.setIsNew(true);
		datastoreObject.allocate();
		return datastoreObject;
	}

	private DatastoreObject existingDatastoreObject(long loid) {
		DatastoreObject datastoreObject = new DatastoreObject(new DatastoreLoid(loid));
		_dm.readObject(datastoreObject, DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
		return datastoreObject;
	}
	
	private DatastoreObject datastoreObjectForUpdate(long loid) {
		DatastoreObject oldDatastoreObject = existingDatastoreObject(loid);
		DatastoreObject newDatastoreObject = new DatastoreObject(loid, oldDatastoreObject.getSchemaClass(), oldDatastoreObject.getDatastoreInfo());
		newDatastoreObject.setTimestamp(oldDatastoreObject.getTimestamp() + 1);
		newDatastoreObject.setIsNew(false);
		newDatastoreObject.allocate();
		return newDatastoreObject;
	}

	public <T> Collection<T> query(Class<T> extent) {
		Object[] loids = datastoreLoids(extent);
		if(loids.length == 0){
			return new ArrayList<T>();
		}
		return readObjects(extent, loids);
	}
	
	public <T> Collection<T> readObjects(Class<T> extent, Object[] loids) {
		return readObjects(extent, loids, -1);
	}

	public <T> Collection<T> readObjects(Class<T> extent, Object[] loids, int limit) {
		int size = limit > 0 ? Math.min(loids.length, limit) : loids.length;
		DatastoreObject[] datastoreObjects = new DatastoreObject[size];
		for ( int i = 0; i < size; i++ ){
			datastoreObjects[i]= new DatastoreObject((DatastoreLoid) loids[i]);
		}
		_dm.groupReadObjects(datastoreObjects, DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
		CobraField[] fields = fields(extent);
		List<T> result = new ArrayList<T>();
		for ( int i = 0; i < datastoreObjects.length; i++ ) {
			try {
				T obj = extent.newInstance();
				ensureLoidSet(obj, ((DatastoreLoid) loids[i]).value());
				for (int j = 0; j < fields.length; j++) {
					fields[j].read(obj, datastoreObjects[i]);
				}
				result.add(obj);
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
		return result;
	}

	private <T> void ensureLoidSet(T obj, long loid) {
		if(obj instanceof VodLoidAwareObject){
			((VodLoidAwareObject)obj).loid(loid);
		}
	}
	
	public boolean containsLoid(long loid) {
		DatastoreObject datastoreObject = new DatastoreObject(new DatastoreLoid(loid));
		return _dm.readObject(datastoreObject, DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
	}
	
	public <T> T objectByLoid(long loid){
		DatastoreObject datastoreObject = existingDatastoreObject(loid);
		Class<T> clazz = classOf(datastoreObject);
		T result;
		try {
			result = clazz.newInstance();
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
		ensureLoidSet(result, loid);
		CobraField[] fields = fields(clazz);
		for (int j = 0; j < fields.length; j++) {
			fields[j].read(result, datastoreObject);
		}
		return result;
	}

	public DatastoreSchemaClass datastoreSchemaClass(Class clazz) {
		return datastoreSchemaClass(schemaName(clazz.getName()));
	}
	
	public DatastoreSchemaClass datastoreSchemaClass(String name) {
		return _dm.getSchemaEditor().findClass(name, datastoreInfo());
	}
	
	public boolean isKnownClass(Class clazz) {
		return internalSchemaName(clazz.getName()) != null;
	}
	
	public String schemaName(String fullyQualifiedName) {
		return internalSchemaName(fullyQualifiedName);
	}

	private String internalSchemaName(String fullyQualifiedName) {
		int pos = fullyQualifiedName.lastIndexOf(".");
		String simpleName = fullyQualifiedName.substring(pos + 1);
		if(datastoreSchemaClass(fullyQualifiedName) != null){
			return fullyQualifiedName;
		}
		if(datastoreSchemaClass(simpleName) != null){
			return simpleName;
		}
		return null;
	}
	
	public void commit(){
		_dm.commitTransaction();
		_dm.beginTransaction();
	}
	
	public void rollback(){
		_dm.rollbackTransaction();
		_dm.beginTransaction();
	}
	
	private CobraField[] fields(Class clazz){
		DatastoreSchemaClass datastoreSchemaClass = datastoreSchemaClass(clazz);
		DatastoreSchemaField[] datastoreSchemaFields = datastoreSchemaClass.getFields();
		CobraField[] cobraFields = new CobraField[datastoreSchemaFields.length];
		for (int i = 0; i < datastoreSchemaFields.length; i++) {
			cobraFields[i] = new CobraField(clazz, datastoreSchemaFields[i]);
		}
		return cobraFields;
	}
	
	public <T> T singleInstanceOrDefault(Class<T> extent, T defaultValue){
		long loid = singleInstanceLoid(extent);
		if(loid == INVALID_LOID){
			return defaultValue;
		}
		return this.<T>objectByLoid(loid);
	}
	
	public <T> T singleInstance(Class<T> extent){
		long loid = singleInstanceLoid(extent);
		if(loid == INVALID_LOID){
			throw new IllegalStateException("No object of " + extent + " stored" );
		}
		return this.<T>objectByLoid(loid);
	}
	
	private <T> long singleInstanceLoid(Class<T> extent) {
		Collection<Long> loids = loids(extent);
	    switch(loids.size()){
	    	case 0:
		    	return INVALID_LOID;
	    	case 1:
	    		return loids.iterator().next();
	    	default:
	    		throw new IllegalStateException("Multiple " + extent.getSimpleName() + " instances in database: " + loids.size());
	    }
	}
	
	private class CobraField {
		
		private DatastoreSchemaField _datastoreSchemaField;
		
		private java.lang.reflect.Field _field;
		
		public CobraField(Class clazz, DatastoreSchemaField datastoreSchemaField){
			_datastoreSchemaField = datastoreSchemaField;
			_field = Reflection4.getField(clazz, name());
		}

		public void write(Object obj, DatastoreObject datastoreObject) {
			Object fieldValue = Reflection4.getFieldValue(obj, name());
			if(isCobraPersistentObject()){
				if(fieldValue == null){
					datastoreObject.writeObject(_datastoreSchemaField, 0);
				} else {
					datastoreObject.writeObject(_datastoreSchemaField, store(fieldValue));
				}
			} else {
				datastoreObject.writeObject(_datastoreSchemaField, fieldValue);
			}
		}
		
		private String name(){
			return _datastoreSchemaField.getName();
		}
		
		public boolean isCobraPersistentObject(){
			return VodLoidAwareObject.class.isAssignableFrom(_field.getType());
		}
		
		public void read(Object obj, DatastoreObject datastoreObject) {
			if(_field == null){
				return;
			}
			try {
				Object readObject = datastoreObject.readObject(_datastoreSchemaField);
				if(isCobraPersistentObject()){
					Long loid = (Long)readObject;
					if(loid > 0){
						readObject = objectByLoid(loid);
					}
				}
				_field.set(obj, readObject);
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	public <T> QLin<T> from(Class<T> clazz) {
		return new CLinRoot<T>(this, clazz);
	}

	public short databaseId(){
		return datastoreInfo().getDBID();	
	}
	
	public String databaseName(){
		return _vod.name();
	}

	public void delete(long loid) {
		
		// TODO this needs an RPC call to read the object
		//      Find out if Cobra has a nicer asynchronous delete 
		//      that just takes a long
		_dm.deleteObject(existingDatastoreObject(loid));
	}
	
	public void delete(Object obj) {
		if(! (obj instanceof VodLoidAwareObject)){
			throw new IllegalArgumentException("Can only delete instanceof VodLoidAwareObject");
		}
		long loid = ((VodLoidAwareObject)obj).loid();
		delete(loid);
	}

	public void deleteAll() {
		rollback();
		Collection<String> classNames = classNames(false);
		String[] classNameArr = classNames.toArray(new String[classNames.size()]);
		for (int i = 0; i < classNameArr.length; i++) {
		    DatastoreQuery qry = new DatastoreQuery(classNameArr[i]);
		    Object[] loids = executeQuery(qry);
		    for (int j = 0; j < loids.length; j++) {
		    	DatastoreLoid datastoreLoid = (DatastoreLoid) loids[j];
		    	delete(datastoreLoid.value());
			}
		}
	}
	
	private Collection<String> classNames(boolean all) {
	      Set<String> classNames = new HashSet<String>();
	      String classclazzNam = CLASS_CLASS_NAME;
	      DatastoreObject[] dsos = datastoreObjects(classclazzNam);
	      DatastoreSchemaClass dsc = _dm.getSchemaEditor().findClass(classclazzNam, datastoreInfo());
	      DatastoreSchemaField dsf = dsc.findField(CLASS_NAME_FIELD_NAME);
	      for(int i = 0; i < dsos.length;i++){
	    	  String className = dsos[i].readObject(dsf).toString();
	    	  if(all || isUserClassName(className)){
	    		  classNames.add(className);
	    	  }
	      }
	      
	      return classNames;
	}

	private boolean isUserClassName(String className) {
		
  	  // For now we distinguish user classes from internal 
  	  // classes by determining if they have a package name.

		return className.contains(".");
	}
	
	public static void deleteAll(VodDatabase vod) {
		VodCobraFacade cobra = VodCobra.createInstance(vod);
		cobra.deleteAll();
		cobra.commit();
		cobra.close();
	}
	
	private DatastoreObject[] datastoreObjects(String className) {
		Object[] loids = executeQuery(new DatastoreQuery(className));
	    DatastoreObject[] dsos = _dm.getLoidsAsDSO(loids);
	    _dm.groupReadObjects(dsos, DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
		return dsos;
	}
	
	public Object[] executeQuery(DatastoreQuery query) {
		return _dm.executeQuery(query, DataStoreLockMode.NOLOCK,
				DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
	}
	
	private Object[] datastoreLoids(Class<?> extent) {
		return datastoreLoids(extent.getName());
	}
	
	private Object[] datastoreLoids(String className) {
		return executeQuery(new DatastoreQuery(storedClassName(className)));
	}
	
	public String storedClassName(String className){
		initializeStoredClassNames();
		String storedClassName = lookupStoredClassName(className);
		if(storedClassName != null) {
			return storedClassName;
		}
		_storedClassNames.addAll(classNames(true));
		storedClassName = lookupStoredClassName(className);
		if(storedClassName != null) {
			return storedClassName;
		}
		throw new IllegalStateException("Classname not found: " + className);
	}

	private String lookupStoredClassName(String className) {
		if(_storedClassNames.contains(className)){
			return className;
		}
		String unqualifiedName = unqualifiedName(className);
		if(_storedClassNames.contains(unqualifiedName)){
			return unqualifiedName;
		}
		return null;
	}
	private void initializeStoredClassNames() {
		if(_storedClassNames != null){
			return;
		}
		_storedClassNames = new HashSet<String>();
		_storedClassNames.addAll(classNames(true));
	}

	private String unqualifiedName(String className) {
		int index = className.lastIndexOf(".");
		if(index > 0){
			return className.substring(index + 1);
		}
		return className;
	}
	
	public long queryForMySignatureLoid(){
		byte[] signatureBytes = signatureBytes(databaseId());
		// TODO: add direct querying for byte[]
		ObjectSet<DatabaseSignature> signatures = from(DatabaseSignature.class).select();
		for (DatabaseSignature signature : signatures) {
			if( Arrays.equals(signatureBytes, signature.signature())){
				return signature.loid();
			}
		}
		return 0;
	}
	
	public byte[] signatureBytes(long databaseId){
		return new LatinStringIO().write("vod-" + databaseId);
	}
	
	public long[] loidsForStoredObjectsOfClass(String className){
		Object[] datastoreLoids = datastoreLoids(className);
		long[] result = new long[datastoreLoids.length];
		for (int i = 0; i < datastoreLoids.length; i++) {
			DatastoreLoid datastoreLoid = (DatastoreLoid) datastoreLoids[i];
			result[i] = datastoreLoid.value();
		}
		return result;
	}

	@Override
	public boolean lockClass(Class<?> clazz) {
		try{
			_dm.lockObject(datastoreObjectFor(clazz), DataStoreLockMode.WLOCK, Options.NO_WAIT_OPTION);
			return true;
		} catch(DatastoreException exc) {
	        if( exc.getDatastoreErrorCode() == DatastoreErrorCodes.SM_LOCK_WOULDBLOCK ) {
	        	return false;
	        }
	        throw exc;
		}
	}

	@Override
	public void unlockClass(Class<?> clazz) {
		try{
			DatastoreObject dso = datastoreObjectFor(clazz);
			_dm.lockObject(dso, DataStoreLockMode.NOLOCK, Options.DOWNGRADE_LOCKS_OPTION);
		} catch(DatastoreException exc) {
	        if( exc.getDatastoreErrorCode() == DatastoreErrorCodes.SM_LOCK_WOULDBLOCK ) {
	        	exc.printStackTrace();
	        }
	        throw exc;
		}
	}
	
	private DatastoreObject datastoreObjectFor(Class<?> clazz) {
		DatastoreSchemaClass datastoreSchemaClass = datastoreSchemaClass(clazz);
		long loid = datastoreSchemaClass.getLOID();
		DatastoreObject datastoreObject = new DatastoreObject(loid);
		datastoreObject.setDatastoreInfo(datastoreInfo());
		return datastoreObject;
	}

	@Override
	public <T> T withLock(final Class<?> clazz, Closure4<T> closure) {
		retryWithTimeOut(new Closure4<Boolean>() {
			@Override
			public Boolean run() {
				return lockClass(clazz);
			}
		}, LOCK_TIMEOUT, WAIT_BEFORE_LOCK_RETRY);
		try {
			return closure.run();
		} finally {
			unlockClass(clazz);
		}
	}

	private static void retryWithTimeOut(Closure4<Boolean> closure, long timeout, int waitBetweenRetry) {
		if(closure.run()){
			return;
		}
		long start = System.currentTimeMillis();
		while(! closure.run()){
			if(System.currentTimeMillis() - start > timeout){
				throw new TimeoutException();
			}
			try {
				Thread.sleep(waitBetweenRetry);
			} catch (InterruptedException e) {
				throw new RuntimeException(e);
			}
		}
	}
	
	public long defaultSignatureLoid() {
		if (_defaultSignatureLoid > 0) {
			return _defaultSignatureLoid;
		}
		_defaultSignatureLoid = queryForMySignatureLoid();
		if (_defaultSignatureLoid != 0) {
			return _defaultSignatureLoid;
		}
		return withLock(DatabaseSignature.class, new Closure4<Long>() {
			@Override
			public Long run() {
				DatabaseSignature databaseSignature = new DatabaseSignature(signatureBytes(databaseId()));
				store(databaseSignature);
				commit();
				_defaultSignatureLoid = databaseSignature.loid();
				return _defaultSignatureLoid;
			}
		});
	}
	
	public ClassMetadata classMetadata(Class clazz){
		ClassMetadata classMetadata = _classToClassMetadata.get(clazz);
		if(classMetadata != null){
			return classMetadata;
		}
		return ensureClassMetadata(clazz);
	}
	
	private ClassMetadata ensureClassMetadata(final Class clazz) {
		final String fullyQualifiedName = clazz.getName();
		final String schemaName = schemaName(fullyQualifiedName);
		ClassMetadata classMetadata = queryForClassMetadata(clazz, schemaName);
		if(classMetadata != null){
			return classMetadata;
		}
		return withLock(ClassMetadata.class, new Closure4<ClassMetadata>() {
			@Override
			public ClassMetadata run() {
				
				ClassMetadata classMetadata = queryForClassMetadata(clazz, schemaName);
				if(classMetadata != null){
					return classMetadata;
				}
				classMetadata = new ClassMetadata(schemaName, fullyQualifiedName);
				store(classMetadata);
				commit();
				_classToClassMetadata.put(clazz, classMetadata);
				return classMetadata;
			}
		});
	}

	private ClassMetadata queryForClassMetadata(Class clazz, String schemaName) {
		ClassMetadata classMetadata = prototype(ClassMetadata.class);
		ClassMetadata storedClassMetadata = from(ClassMetadata.class).where(classMetadata.name()).equal(schemaName).singleOrDefault(null);
		if(storedClassMetadata != null){
			_classToClassMetadata.put(clazz, storedClassMetadata);
			return storedClassMetadata;
		}
		return null;
	}

	@Override
	public void updateTimestamps(int newTimestamp, long... loids) {
		
		if(loids == null || loids.length == 0){
			return;
		}

		DatastoreObject[] objs = new DatastoreObject[loids.length];
		int count = 0;
		for(long loid : loids) {
			objs[count++] = new DatastoreObject(new DatastoreLoid(loid));
		}
		GroupOperationResult readResult = _dm.groupReadObjects(objs, DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
		if (readResult != null) {
			throw new RuntimeException("Failure in groupReadObjects when updating timestamp.");
		}
		DatastoreObject[] writes = new DatastoreObject[loids.length];

		for(int i=0;i<count;i++) {
			DatastoreObject dso = objs[i];
			DatastoreSchemaClass clazz = dso.getSchemaClass();
			
			DatastoreObject write = new DatastoreObject(dso.getLOID(), clazz, dso.getDatastoreInfo());
			write.allocate();
			
			for (DatastoreSchemaField field : clazz.getAllFields()) {
				DatastoreObject.copyField(dso, field, write, field);
			}
			
			write.setTimestamp(newTimestamp);
			writes[i] = write;
		}

		if (_dm.groupWriteObjects(writes, Options.NO_OPTIONS) != null) {
			throw new RuntimeException("Failure in groupWriteObjects when updating timestamp.");
		}
	}

	@Override
	public int[] getTimestamps(long ...loids) {
		int[] result = new int[loids.length];
		DatastoreObject[] objs = new DatastoreObject[loids.length];
		int count = 0;
		for(long loid : loids) {
			objs[count++] = new DatastoreObject(new DatastoreLoid(loid));
		}
		GroupOperationResult readResult = _dm.groupReadObjects(objs, DataStoreLockMode.NOLOCK, Options.NO_OPTIONS);
		if (readResult != null) {
			throw new RuntimeException("Failure in groupReadObjects when updating timestamp.");
		}
		for(int i=0;i<count;i++) {
			DatastoreObject dso = objs[i];
			result[i] = dso.getTimestamp();
		}
		return result;
	}

}
