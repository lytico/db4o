package com.db4o.omplus.datalayer.queryresult;

import java.util.*;

import com.db4o.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.queryBuilder.*;
import com.db4o.reflect.*;

/**
 * Implemented for Query Results Tab or class
 *
 */

@SuppressWarnings("unchecked")
public class QueryResultsManager
{	
	private	long[] resultObjIds;
	private	String	 className;
	private String[] displayFields;
	
	private OMQuery oMQuery;
	
	private IDbInterface db = Activator.getDefault().dbModel().db();
	
	public QueryResultsManager() {
	}

	public String getClassname() {
		return className;
	}

	public void setClassname(String classname) {
		this.className = classname;
	}
	
	public void runQuery(OMQuery query){
		if(query != null){
			oMQuery = query;
			QueryParser parser = new QueryParser();
			className = query.getQueryClass();
			displayFields = query.getAttributeList();
			ObjectSet set = parser.executeOMQueryList(query);
			resultObjIds = set.ext().getIDs();
		}
	}
	
	public QueryResultList getResultList(){
		QueryResultList list = new QueryResultList(db.reflectHelper());
		if(displayFields == null || (displayFields.length == 0) ){
			displayFields = getAllFieldNames();
		}
		ArrayList<Long> idList = new ArrayList<Long>(resultObjIds.length);
		for(long id : resultObjIds){
			idList.add(id);
		}
		list.setLocalIdList(idList);
		list.setOMQuery(oMQuery);
		list.setFieldNames(displayFields);
		list.setClassName(className);
		return list;
	}

	private String[] getAllFieldNames() 
	{
		ReflectClass clazz = getReflectClazz(className);
		ReflectField rField[] =ReflectHelper.getDeclaredFieldsInHierarchy(clazz);
		displayFields = new String[rField.length];
		StringBuilder sb = null;
		for(int idx = 0; idx < displayFields.length ; idx++)
		{
			sb = new StringBuilder(16);
			sb.append(className);sb.append(OMPlusConstants.REGEX);
			sb.append(rField[idx].getName());
			displayFields[idx] = sb.toString();
		}
		return displayFields;
	}

	private ReflectClass getReflectClazz(String className)
	{
		return db.getDB().ext().reflector().forName(className);
	}

	public void runQuery(String name, String []fieldNames)
	{
		ReflectClass clazz = db.reflectHelper().getReflectClazz(name);
		if(clazz != null)
		{
			setClassname(name);
			if(fieldNames != null && fieldNames.length > 0)
				displayFields = fieldNames;
			OMQuery query = buildQuery(name);
			query.setAttributeList(fieldNames);
			QueryParser parser = new QueryParser();
			ObjectSet set = parser.execute(clazz);
			oMQuery = query;
			resultObjIds = set.ext().getIDs();
		}
	}

	private OMQuery buildQuery(String classname)
	{
		OMQuery query = new OMQuery();
		query.setQueryClass(classname);
		return query;
	}
}
