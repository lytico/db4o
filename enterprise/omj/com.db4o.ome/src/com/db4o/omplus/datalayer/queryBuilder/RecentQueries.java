package com.db4o.omplus.datalayer.queryBuilder;

import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;

public class RecentQueries {
	
	private final String KEY_GQ = "GLOBAL_QUERIES";

	public List<OMQuery> getRecentQueriesForDB() {
		OMEDataStore data = Activator.getDefault().getOMEDataStore();
		List<OMQuery> queryList = data.getContextLocalEntry(KEY_GQ);
	    if (queryList == null){
	    	queryList = new ArrayList<OMQuery>();
	    }
	 	return queryList;
	}
	
	 public void addNewDBQuery(OMQuery query) {
		// make sure it's not already here
		 boolean modify = true;
		 if(query != null){
			 List<OMQuery> queryList = getRecentQueriesForDB();
			 modify = canAddQuery(query, queryList);
			 if(modify)
				 queryList.add(query);
			 if(queryList.size() > 10)
				 queryList.remove(0);
			 saveRecentQueryList(queryList);
		 }
	}
	 
	private boolean canAddQuery(OMQuery query, List<OMQuery> queryList){
		boolean canAddQuery = true;
		int size = queryList.size();
		 if(size > 0){
			 String queryStr = query.toString();
			 for (int i = size-1; i > -1 ; i--) {
				 OMQuery rcQuery = (OMQuery) queryList.get(i);
				 String rcQueryStr = rcQuery.toString();
					if (rcQueryStr.equals(queryStr)) {
						if( i == size - 1) {
							canAddQuery = false;
							break;
						}
						else
							queryList.remove(rcQuery);
					}
			 }				
		 }
		return canAddQuery;
	}

	private void saveRecentQueryList(List<OMQuery> queryList) {
		OMEDataStore data = Activator.getDefault().getOMEDataStore();
		data.setContextLocalEntry(KEY_GQ, queryList);
	}
	
	/*	@SuppressWarnings("unchecked")
	public ArrayList<OMQuery> getRecentQueriesForClass(String className) {
		if(className != null){
			ArrayList<OMQuery> queryList = data.getDataValue(className);
		    if (queryList == null){
		    	queryList = new ArrayList<OMQuery>();
		    }
		 	return queryList;
		}
		return null;
	}
	
	 public void addNewQueryForClass(String className, OMQuery query) {
	        // make sure it's not already here
		 boolean modify = true;
		 if(className != null){
			 ArrayList<OMQuery> queryList = getRecentQueriesForClass(className);
			 modify = canAddQuery(query, queryList);
			 if(modify)
				 queryList.add(query);
			 if(queryList.size() > 5)
				 queryList.remove(0);
			 saveRecentQueryForCls(className, queryList);
		 }
	}

	private void saveRecentQueryForCls(String className, ArrayList<OMQuery> queryList) {
		data.setDataValue(className, queryList);
	}*/
}
