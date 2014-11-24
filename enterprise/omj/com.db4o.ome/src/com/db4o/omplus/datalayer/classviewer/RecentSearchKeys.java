package com.db4o.omplus.datalayer.classviewer;

import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;

public class RecentSearchKeys {

	private final String SEARCH_KEY = "SEARCH_KEY";
	
//	private OMEData data = Activator.getDefault().getOMEDataStore();
	
	public List<String> getSearchKeysForDB() {
		OMEDataStore data = Activator.getDefault().getOMEDataStore();
		List<String> queryList = data.getContextLocalEntry(SEARCH_KEY);
	    if (queryList == null){
	    	queryList = new ArrayList<String>(11);
	    }
	 	return queryList;
	}
	
	 public void addNewSearchKey(String query) {
		// make sure it's not already here
		 boolean modify = true;
		 if(query != null){
			 List<String> queryList = getSearchKeysForDB();
			 modify = canAddQuery(query, queryList);
			 if(modify)
				 queryList.add(query);
			 if(queryList.size() > 10)
				 queryList.remove(0);
			 saveSearchKeys(queryList);
		 }
	}
	 
	private boolean canAddQuery(String query, List<String> queryList){
		boolean canAddQuery = true;
		int size = queryList.size();
		 if(size > 0){
			 String queryStr = query.toString();
			 for (int i = size-1; i > -1 ; i--) {
				String rcQuery = (String) queryList.get(i);
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

	private void saveSearchKeys(List<String> queryList) {
		OMEDataStore data = Activator.getDefault().getOMEDataStore();
		data.setContextLocalEntry(SEARCH_KEY, queryList);
	}
	
/*	public ArrayList<String> getRecentQueriesForClass(String className) {
		if(className != null){
			ArrayList<String> queryList = data.getDataValue(className);
		    if (queryList == null){
		    	queryList = new ArrayList<String>();
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
