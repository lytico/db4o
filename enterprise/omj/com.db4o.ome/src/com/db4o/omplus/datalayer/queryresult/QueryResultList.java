package com.db4o.omplus.datalayer.queryresult;

import java.util.ArrayList;

import com.db4o.omplus.datalayer.ReflectHelper;
import com.db4o.omplus.datalayer.queryBuilder.OMQuery;

public class QueryResultList {
	
	private String className;
	
	private	String[] fieldNames;
	
	private OMQuery oMQuery;
	
	private ArrayList<Long> localIdList;

	private final ReflectHelper reflectHelper;

	public QueryResultList(ReflectHelper reflectHelper) {
		this.reflectHelper = reflectHelper;
	}
	
	public String[] getFieldNames()
	{
		return fieldNames;
	}
	
	public int getFieldIndex(String fName) {
		if( fName != null && !(fName.equals(""))){
			int index = 0;
			for(String field:fieldNames){
				if(fName.equals(field))
					return index;
				index++;
			}
		}
		return -1;
	}
	
	public int size(){
		if(localIdList != null){
			return localIdList.size();
		}
		return 0;
	}

	public void setFieldNames(String[] fieldNames) {
		this.fieldNames = fieldNames;
	}

	public String getClassName() {
		return className;
	}

	public void setClassName(String className) {
		this.className = className;
	}

	public OMQuery getOMQuery() {
		return oMQuery;
	}

	public void setOMQuery(OMQuery oMQuery) {
		this.oMQuery = oMQuery;
	}

	//Given a fieldName return true if the adatype of that field name is boolean
	/*public boolean isBoolean(String fieldName)
	{
		if( fieldName != null ){
			int type = ReflectHelper.getFieldTypeClass(fieldName);
			if(type == QueryBuilderConstants.DATATYPE_BOOLEAN)
				return true;
		}
		return false;
	}
*/
	
	public int getDataType(String fieldName)
	{
		if( fieldName != null ){
			int type = reflectHelper.getFieldTypeClass(fieldName);
			return type;
		}
		return -1;
	}
	
	public ArrayList<Long> getLocalIdList() {
		return localIdList;
	}

	public void setLocalIdList(ArrayList<Long> localIdList) {
		this.localIdList = localIdList;
	}
	
	public void removeFromList(int index){
		localIdList.remove(index);
	}
	

}