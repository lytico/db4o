package com.db4o.omplus.datalayer.queryresult;

import java.util.*;

import com.db4o.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;

@SuppressWarnings("unchecked")
public class QueryResultTableModifiedList
{
	
	private HashSet <Object>modifiedObjList;
	
	private List <Object>deleteObjList;
	
	public QueryResultTableModifiedList()
	{
		modifiedObjList = new HashSet<Object>();
		deleteObjList = new ArrayList<Object>();
	}
	
	public HashSet<Object> getModifiedObjList()
	{
		return modifiedObjList;
	}
	
	public void addObjectToList(Object obj)
	{
		if(obj!=null)
			modifiedObjList.add(obj);
	}
	
	public void clearModifiedObjList()
	{
		int length = modifiedObjList.size();
		if(length > 0){
			modifiedObjList.clear();
		}
		
	}
	
	public void clearDeletedObjList()
	{
		int length = deleteObjList.size();
		if(length > 0){
			deleteObjList.clear();
		}
	}
	
	public void writeToDB()
	{
		ObjectContainer db = getOC();
		if(db != null){
			Iterator<Object> iterator = modifiedObjList.iterator();
			while(iterator.hasNext())
			{
				Object obj = iterator.next();
				db.store(obj);	
			}
			db.commit();
			clearModifiedObjList();
		}
	}
		
	public void deleteFromDB()
	{
		ObjectContainer db = getOC();
		if(db != null){
			Iterator iterator = deleteObjList.iterator();			
			while(iterator.hasNext()) 
			{
				Object keyObj = iterator.next();
				db.delete(keyObj);
				db.ext().purge(keyObj);
			}
			db.commit();
			clearDeletedObjList();
		}
	}
	

	public List<Object> getDeleteObjList() 
	{
		return deleteObjList;
	}

	public void setDeleteObjList(List<Object> deleteObjList) 
	{
		this.deleteObjList = deleteObjList;
	}

	public void addToDeleteList(Object obj)
	{
		if(obj!=null )
			deleteObjList.add(obj);
	}

	public void refresh()
	{
		ObjectContainer db = getOC();
		if(db != null){
			Iterator<Object> iterator = modifiedObjList.iterator();
			while(iterator.hasNext())
			{
				Object obj = iterator.next();
				db.ext().refresh(obj, 2);			
			}
			clearModifiedObjList();
		}
	}
	
	private ObjectContainer getOC()
	{
		return Activator.getDefault().dbModel().db().getDB();
	}
}
