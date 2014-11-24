package com.db4o.drs.versant;

import java.util.*;

import com.db4o.drs.versant.metadata.*;
import com.db4o.foundation.*;
import com.db4o.qlin.*;
import com.versant.odbms.query.*;

public interface VodCobraFacade {

	void close();

	long store(Object obj);
	
	void create(long loid, Object obj);

	void store(long loid, Object obj);

	Collection<Long> loids(Class<?> extent);

	<T> Collection<T> query(Class<T> extent);

	<T> Collection<T> readObjects(Class<T> extent, Object[] loids);

	<T> Collection<T> readObjects(Class<T> extent, Object[] loids, int limit);

	<T> T objectByLoid(long loid);
	
	boolean containsLoid(long loid);

	void commit();

	void rollback();

	<T> T singleInstanceOrDefault(Class<T> extent, T defaultValue);

	<T> T singleInstance(Class<T> extent);

	<T> QLin<T> from(Class<T> clazz);

	Object[] executeQuery(DatastoreQuery query);

	short databaseId();

	String databaseName();

	void delete(long loid);
	
	void delete(Object obj);
	
	void deleteAll();
	
	public String schemaName(String fullyQualifiedName);
	
	boolean isKnownClass(Class clazz);
	
	long queryForMySignatureLoid();
	
	byte[] signatureBytes(long databaseId);

	long[] loidsForStoredObjectsOfClass(String className);
	
	public void produceSchema(Class<?> clazz);

	boolean lockClass(Class<?> clazz);

	void unlockClass(Class<?> clazz);

	<T> T withLock(Class<?> clazz, Closure4<T> closure);
	
	String storedClassName(String className);
	
	ClassMetadata classMetadata(Class clazz);
	
	long defaultSignatureLoid();

	void updateTimestamps(int newTimestamp, long... loids);

	int[] getTimestamps(long... objectLoid);
	
	
}
