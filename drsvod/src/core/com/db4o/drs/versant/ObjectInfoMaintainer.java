/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.drs.versant.metadata.ObjectInfo.*;
import com.db4o.drs.versant.timestamp.*;
import com.versant.odbms.*;
import com.versant.odbms.query.*;
import com.versant.odbms.query.Operator.UnaryOperator;
import static com.db4o.qlin.QLinSupport.*;

public class ObjectInfoMaintainer {

	public static final int OBJECTINO_STORED_THRESHOLD = Integer.MAX_VALUE / 2;
	
	private final VodCobraFacade _cobra;
	
	private final TimestampGenerator _timestampGenerator;
	
	private final List<Long> _newLoids = new ArrayList<Long>();

	public ObjectInfoMaintainer(VodCobraFacade cobra) {
		_cobra = cobra;
		_timestampGenerator = new TimestampGenerator(cobra);
		System.err.println("TODO: Replace TimestampGenerator with more efficient algorithm that reserves timestamps and doesn't call commit for every timestamp.");
	}
	
	public void updateObjectInfos(Class<?> clazz, long updateToVersion) {
		ensureObjectInfosExist(clazz, updateToVersion);
		ensureObjectInfosInSync(clazz, updateToVersion);
		_cobra.commit();
	}

	private void ensureObjectInfosInSync(Class<?> clazz, long updateToVersion) {
		long classMetadataLoid = _cobra.classMetadata(clazz).loid();
		ObjectInfo info = prototype(ObjectInfo.class);
		ObjectSet<ObjectInfo> objectSet = _cobra.from(ObjectInfo.class).where(info.classMetadataLoid()).equal(classMetadataLoid).select();
		int count = 0;
		ObjectInfo[] objectInfos = new ObjectInfo[objectSet.size()];
		long[] loids = new long[objectSet.size()];
		Iterator<ObjectInfo> it = objectSet.iterator();
		while(it.hasNext()){
			objectInfos[count] = it.next();
			loids[count] = objectInfos[count].objectLoid();
			count++;
		}
		int[] timestamps = _cobra.getTimestamps(loids);
		for (int i = 0; i < loids.length; i++) {
			ObjectInfo objectInfo = objectInfos[i];
			if(timestamps[i] > objectInfo.internalTimestamp()){
				objectInfo.internalTimestamp(timestamps[i]);
				objectInfo.version(updateToVersion);
			}
			_cobra.store(objectInfo);
		}
	}

	private void ensureObjectInfosExist(Class<?> clazz, long versionForNewObjectInfo) {
		DatastoreQuery query = new DatastoreQuery(_cobra.storedClassName(clazz.getName()));
		Expression expression = new Expression(
				new SubExpression(new Field("o_ts_timestamp")),
				UnaryOperator.LESS_THAN,
				new SubExpression(OBJECTINO_STORED_THRESHOLD));
		query.setExpression(expression);
		Object[] loids = _cobra.executeQuery(query);
		
		long classMetadataLoid = _cobra.classMetadata(clazz).loid();
		
		// FIXME: Group write all changes to improve performance.
		System.err.println("TODO: group write objects");
		
		long[] loidsAsLong = new long[loids.length];
		
		for (int i = 0; i < loids.length; i++) {
			loidsAsLong[i] = ((DatastoreLoid)loids[i]).value();
			ObjectInfo objectInfo = new ObjectInfo(
					_cobra.defaultSignatureLoid(), 
					classMetadataLoid, 
					loidsAsLong[i], 
					_timestampGenerator.generate(), 
					versionForNewObjectInfo, 
					Operations.CREATE.value,
					0);
			_cobra.store(objectInfo);
		}
		_cobra.updateTimestamps(OBJECTINO_STORED_THRESHOLD, loidsAsLong);
	}
	
	public void addNewLoid(long loid){
		_newLoids.add(loid);
	}
	
	public void commitNewLoids(){
		long[] newLoidsArray = new long[_newLoids.size()];
		for (int i = 0; i < newLoidsArray.length; i++) {
			newLoidsArray[i] = _newLoids.get(i);
		}
		_cobra.updateTimestamps(OBJECTINO_STORED_THRESHOLD, newLoidsArray);
		_cobra.commit();
		_newLoids.clear();
	}
	
	

}
