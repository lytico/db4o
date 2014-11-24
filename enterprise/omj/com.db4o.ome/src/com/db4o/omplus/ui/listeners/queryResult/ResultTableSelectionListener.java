package com.db4o.omplus.ui.listeners.queryResult;

import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.widgets.TableItem;

import com.db4o.omplus.datalayer.queryresult.QueryResultList;
import com.db4o.omplus.datalayer.queryresult.QueryResultRow;
import com.db4o.omplus.ui.customisedcontrols.queryResult.ObjectViewer;
import com.db4o.omplus.ui.interfaces.IViewUpdatorForQueryResults;

public class ResultTableSelectionListener implements SelectionListener 
{
	private QueryResultList queryResultList;
	private ObjectViewer objectViewer;
	private IViewUpdatorForQueryResults viewUpdator;
	

	public ResultTableSelectionListener(QueryResultList q, ObjectViewer o, IViewUpdatorForQueryResults updator) 
	{
		this.queryResultList = q;
		this.objectViewer = o;
		this.viewUpdator = updator;
	}

	public void widgetDefaultSelected(SelectionEvent e) {
		//  Auto-generated method stub
		
	}

	public void widgetSelected(SelectionEvent e) 
	{
		TableItem item  = (TableItem)e.item;
		QueryResultRow row = (QueryResultRow)item.getData();
		objectViewer.addTabsInObjectViewer(row,queryResultList.getClassName(), item.getText(0));

		viewUpdator.updatePropertiesView(queryResultList.getClassName());
		viewUpdator.updatePropertiesView(row.getResultObj());
		
	}

}
