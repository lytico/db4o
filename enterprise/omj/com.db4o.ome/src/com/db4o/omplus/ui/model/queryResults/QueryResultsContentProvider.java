package com.db4o.omplus.ui.model.queryResults;



import java.util.ArrayList;

import org.eclipse.jface.viewers.IStructuredContentProvider;
import org.eclipse.jface.viewers.Viewer;

import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.queryresult.QueryResultList;
import com.db4o.omplus.datalayer.queryresult.QueryResultRow;

public class QueryResultsContentProvider implements IStructuredContentProvider 
{
	
	// testing for paging
	private QueryResultPage page;
	private ArrayList<Long> resultIds;
	
	public QueryResultsContentProvider(QueryResultPage page){
		this.page = page;
	}

	@SuppressWarnings("unchecked")
	public Object[] getElements(Object inputElement) 
	{
		if (inputElement instanceof QueryResultList) 
		{			
			QueryResultList rowList = (QueryResultList)inputElement;
			if(rowList != null && rowList.size() > 0) {
				int pageToBeDisplayed = 1;
				if(page != null){
					pageToBeDisplayed = page.getCurrentPage();
				}
				int startIdx = (pageToBeDisplayed - 1) * OMPlusConstants.MAX_OBJS_PAGE;
				@SuppressWarnings("unused")
				int arrLength = OMPlusConstants.MAX_OBJS_PAGE;
				int length = rowList.size() - startIdx;
				if(length < 50)
					arrLength = length;
				resultIds = rowList.getLocalIdList();
				ArrayList<QueryResultRow> list = page.getObjectsForIds(rowList.getClassName(),
														resultIds, startIdx, rowList.getFieldNames());
				return list.toArray();
			}
		}
		return new Object[0];
	}

	public void dispose() {
		// TODO Auto-generated method stub
		
	}

	public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
		// TODO Auto-generated method stub
		
	}

}
