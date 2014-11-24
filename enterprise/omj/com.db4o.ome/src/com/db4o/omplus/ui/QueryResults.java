
package com.db4o.omplus.ui;

import java.util.Hashtable;
import java.util.Iterator;

import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CTabFolder;
import org.eclipse.swt.custom.CTabFolder2Adapter;
import org.eclipse.swt.custom.CTabFolderEvent;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.layout.FillLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.ui.IViewPart;
import org.eclipse.ui.part.ViewPart;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.queryresult.QueryResultList;
import com.db4o.omplus.ui.customisedcontrols.queryResult.QueryResultTab;
import com.db4o.omplus.ui.interfaces.IViewUpdatorForQueryResults;


public class QueryResults extends ViewPart implements IViewUpdatorForQueryResults
{
	/**
	 * The main tab which stores results for each class
	 */
	private CTabFolder queryResultParentTab;
	
	/**
	 * A map which stores if results for a class have been displayed or not
	 */
	private Hashtable<String, Integer> classIdentifierMap = new Hashtable<String, Integer>(10);
	
	private Composite parentComposite = null;
	
	@Override
	public void createPartControl(Composite parent) 
	{
		parentComposite = parent;
		parentComposite.setLayout(new FillLayout());		
		
		queryResultParentTab = new CTabFolder(parentComposite, SWT.TOP);
		queryResultParentTab.setLayout(new FillLayout());
		queryResultParentTab.addCTabFolder2Listener(new QueryResultTabCloseAdapter());
		
		
		//LOGIC: If a Tab is selected correspondingly update the QueryBuilder tab and poprertiesViewer tab
		queryResultParentTab.addSelectionListener(new SelectionListener()
		{
			public void widgetDefaultSelected(SelectionEvent e) 
			{
			}

			public void widgetSelected(SelectionEvent e) 
			{
				QueryResultTab tab = (QueryResultTab)queryResultParentTab.getSelection();
				
				//For Vieww All Objhects result set...OMQuery null in result Set

				if(tab.getOMQueryForQueryResultTab() == null)
					return;
				// Commented for 31/1/2008 build to stop updating QueryBuilder
				/*IViewPart view = ViewerManager.getView(getViewSite().getPage(),OMPlusConstants.QUERY_BUILDER_ID);
				if(view instanceof QueryBuilder)
				{
					((QueryBuilder)view).resetQueryBuilder(tab.getOMQueryForQueryResultTab());
				}*/
				
				updatePropertiesView(tab.getText());
				
			}
			
		});
	}

	@Override
	public void setFocus() 
	{
	}
	
	
	public void addNewQueryResults(QueryResultList queryResultList)
	{
		int index = tabExists(queryResultList); 
		if(index != -1)
		{ //Class result tab exists show existing tab
			//TODO: check if current tab result set needs to be updated
			
			//Update the queryTab to index -1
			QueryResultTab existingTab = (QueryResultTab)queryResultParentTab.getItem(index);
			updateExistingTab(existingTab,queryResultList);
			
			queryResultParentTab.setSelection(index);
			return;
		}
		
		QueryResultTab queryResultTab = new QueryResultTab(Activator.getDefault().queryModel(), queryResultParentTab,
		   											       SWT.BORDER|SWT.CLOSE, 
		   											       queryResultList,
		   											       this);
		String queryResultClassName = queryResultList.getClassName(); 
		int queryResultIndex = queryResultParentTab.getItemCount()-1;	
		
		classIdentifierMap.put(queryResultClassName,new Integer(queryResultIndex));
		queryResultTab.setText(queryResultClassName);
		queryResultParentTab.setSelection(queryResultIndex);
		addComponentsToQueryResultTab(queryResultTab,queryResultList);
		
		refresh();
		updatePropertiesView(queryResultTab.getText());
	}
	
	/**
	 * Check if tab exists
	 * @param queryResultList
	 * @return
	 */
	private int tabExists(QueryResultList queryResultList) 
	{
		Integer i = classIdentifierMap.get(queryResultList.getClassName());
		if(i == null)
		{
			return -1;
		}
		else
		{
			return i.intValue();
		}
	}

	private void updateExistingTab(QueryResultTab queryResultTab,
									QueryResultList queryResultList)
	{
		addComponentsToQueryResultTab(queryResultTab,queryResultList);
		refresh();
	}
	
	private void addComponentsToQueryResultTab(QueryResultTab queryResultTab,
												QueryResultList queryResultList)
	{
		queryResultTab.addComponentsToTab(queryResultList);

	}
	
	/**
	 * Refresh the view
	 */
	public void refresh()
	{
		parentComposite.layout(true, true);
	}	

	/////////////////////////////////InnerClass: for handling closing tab items
	class QueryResultTabCloseAdapter extends CTabFolder2Adapter
	{
		@SuppressWarnings("unchecked")
		public void close(CTabFolderEvent event) 
		{
			QueryResultTab tab = (QueryResultTab) event.item;
//			tab.beforePageSet();
			tab.beforeTabClose();
			String key = tab.getClassNameForQueryResultTab();
			int tabIndex = classIdentifierMap.get(key);
			classIdentifierMap.remove(key);
			Iterator iterator = classIdentifierMap.keySet().iterator();
			while(iterator.hasNext()) {
				String tempKey = (String)iterator.next();
				int i = classIdentifierMap.get(tempKey);
				if(i > tabIndex){
					i--;
					classIdentifierMap.put(tempKey, i);
				}
			}
		}
	}

	/***************************** BEGIN IViewUpdatorForQueryresult interface ****************************/
	public void updatePropertiesView(String className) 
	{
		IViewPart view = ViewerManager.getView(getViewSite().getPage(),OMPlusConstants.PROPERTY_VIEWER_ID);
		if(view instanceof PropertyViewer)
		{
			((PropertyViewer)view).updateClassProperties(className);
		}
		
	}
	
	public void updatePropertiesView(Object resultObj)
	{
		
		IViewPart view = ViewerManager.getView(getViewSite().getPage(),OMPlusConstants.PROPERTY_VIEWER_ID);
		if(view instanceof PropertyViewer)
		{
			((PropertyViewer)view).updateObjectproperties(resultObj);
		}
		
	}	
	
	/**************************************************  END IViewUpdatorForQueryresult**************************************************/
	
	/**
	 * Reset the Queryresults for a new DB
	 */
	public void resetQueryResultsForNewDB()
	{
		classIdentifierMap = new Hashtable<String, Integer>();
		for(int i = 0; i < queryResultParentTab.getItemCount(); i++)
		{
			queryResultParentTab.getItem(i).dispose();
		}
	}
	
	
	/**
	 * Need to reset QueryBuilder when all tabs closed
	 */
	public void resetQueryBuilder()
	{
		
		IViewPart view = ViewerManager.getView(getViewSite().getPage(),OMPlusConstants.QUERY_BUILDER_ID);
		if(view instanceof QueryBuilder)
		{
			((QueryBuilder)view).allResultsTabClosedInQueryResults();
		}
		
	}
	
	
	//TODO: because of recusrion problem not calloign this function
	/**
	 * This tab has been activated
	 */
	
	public void activated()
	{
		//QueryResults activated
		
		//LOGIC: First time this view not created no need to do anything. 
		//Needed when RunQuery btn clicked and corr ACtivator's PartListener also 
		//called simulataneously 
		if(queryResultParentTab==null || queryResultParentTab.getItemCount()==0)
			return;
		
		
		QueryResultTab tab = (QueryResultTab)queryResultParentTab.getSelection();
		System.out.println(tab.getText());
		
		//1. Update the QueryBuilder with correct OmQuery
		IViewPart view = ViewerManager.getView(getViewSite().getPage(),OMPlusConstants.QUERY_BUILDER_ID);
		if(view instanceof QueryBuilder)
		{
			((QueryBuilder)view).resetQueryBuilder(tab.getOMQueryForQueryResultTab());
		}
		//2. Update the propertiesView with new class
		updatePropertiesView(tab.getText());
	}
	
	
}
