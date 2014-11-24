package com.db4o.omplus.ui.model.queryResults;

import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.queryresult.*;
import com.db4o.reflect.*;

public class QueryResultPage 
{	
	private int numOfPages;
	
	private int currentPage;
	
	private boolean[]	isFieldEditable;
	
	private boolean refresh;
	
	private IDbInterface db = Activator.getDefault().dbModel().db();

	public int getNumOfPages() {
		return numOfPages;
	}

	public void setNumOfPages(int numOfPages) {
		this.numOfPages = numOfPages;
	}

	public int getCurrentPage() {
		return currentPage;
	}

	public void setCurrentPage(int currentPage) {
		this.currentPage = currentPage;
	}

	public ArrayList<QueryResultRow> getObjectsForIds(String className,ArrayList<Long> ids, int startIdx, String[] displayFields) {
		int length = 0;
		int size =ids.size();
		if((size - startIdx) < OMPlusConstants.MAX_OBJS_PAGE)
			length = size - startIdx;
		else 
			length = OMPlusConstants.MAX_OBJS_PAGE;
		ArrayList<QueryResultRow> rowList = new ArrayList<QueryResultRow>(length);
		int idx = (currentPage - 1) * OMPlusConstants.MAX_OBJS_PAGE;
		IDbInterface db = Activator.getDefault().dbModel().db();
		for(int i = 0; i < length && idx < size ; i++){
			long objId = ids.get(idx);
			Object obj = getObjectById(objId, db);
//			db.activate(obj, 1);
			if(obj != null){
				QueryResultRow row = new QueryResultRow();
				refreshObj(obj);
				getObjectTreeValues(obj, row, className, displayFields);
				row.setResultObj(obj);
				row.setId(idx+1);
				rowList.add(row);
				idx++;
			}
			else {
				ids.remove(idx);
			}
		}
		return rowList;
	}

	private void refreshObj(Object obj) {
		if(refresh) {
			db.refreshObj(obj);
		}
	}

	// currently changed to public. Check refresh in result table
	public Object getObjectById(long objId, IDbInterface db) {
		try{
			return db.getObjectById(objId);
		}catch(Exception ex){
			
		}
		return null;
	}

	private void getObjectTreeValues(Object obj, QueryResultRow row,
			String className, String[] displayFields)
	{
		int size = displayFields.length;
		int valIdx = 0;
		boolean isEditableNull = false;
		boolean isValueNull = false;
		Object []values = new Object[size];
		if(isFieldEditable == null){
			isFieldEditable = new boolean[size];
			isEditableNull = true;
		}
		//set the field only the first time after 
		for(String str : displayFields)
		{
			String[] hierarchy = str.split(OMPlusConstants.REGEX);
			Object resObj = obj;
			int count = 1;
			int length = hierarchy.length;
			db.activate(resObj, length);
			ReflectClass clazz = db.reflectHelper().getReflectClazz(hierarchy[0]);
//			for each displayField i.e. ClzName.field.subClzField type get value for subClzField
			while(count < length && resObj != null)
			{
				
				ReflectField rf = ReflectHelper.getDeclaredFieldInHeirarchy(clazz, hierarchy[count++]);
				resObj = rf.get(resObj);
				refreshObj(resObj);
				clazz = rf.getFieldType();
			}
			if(clazz.isCollection() )
				db.activate(resObj, 1);
			
			if(resObj == null){
				isValueNull = true;
			}
			refreshObj(resObj);
			values[valIdx] = resObj;
			if(isEditableNull)
				isFieldEditable[valIdx] = (clazz.isPrimitive() || ReflectHelper.isWrapperClass(clazz.getName()));
			valIdx++;
		}
		row.setValues(values) ;
//		Temp solution for showing null & setting isEditable null;
		if(isValueNull){
			boolean []isEditable = new boolean[size];
			System.arraycopy(isFieldEditable, 0, isEditable, 0, size);
			for(int i = 0; i< size; i++){
				if(values[i] == null){
					values[i] = new String(OMPlusConstants.NULL_VALUE);
				}
			}
			row.setIsCellModifiable(isEditable);
		} 
		else
			row.setIsCellModifiable(isFieldEditable);
	}

	public void refresh(boolean toBeRefreshed) {
		refresh = toBeRefreshed;
	}

}
