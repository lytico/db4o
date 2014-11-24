package com.db4o.omplus.ui;

import org.eclipse.ui.IViewPart;
import org.eclipse.ui.IWorkbenchPage;
import org.eclipse.ui.PartInitException;
import org.eclipse.ui.PlatformUI;

import com.db4o.omplus.datalayer.OMPlusConstants;

/**
 * An abstract class that is used to manage views.
 */

public abstract class ViewerManager 
{
	
	/**
	 * Hides the view specified by the viewId
	 * @param page
	 * @param viewId
	 */
	public static void hideView(IWorkbenchPage page, String viewId)
	{
		IViewPart view = getView(page, viewId);
		page.hideView(view);		
	}
	
	/**
	 * Show the view specified by the viewId
	 * @param page
	 * @param viewId
	 */
	public static void showView(IWorkbenchPage page, String viewId)
	{
		try
		{
			page.showView(viewId);
		}
		catch (PartInitException e) 
		{
			// TODO Exception handling additioon....show dialog box
			e.printStackTrace();
		}
		
	}
	
	/**
	 * Returns a view with teh specific viewId
	 * @param page
	 * @param viewId
	 * @return
	 */
	public static IViewPart getView(IWorkbenchPage page, String viewId)
	{
		IViewPart  view = page.findView(viewId);
		
		if(view == null)// changed ==
		{
			try 
			{
				if(viewId.equals(OMPlusConstants.QUERY_RESULTS_ID))// fix for OMJ-9
					page.showView(viewId);
				view = page.findView(viewId);
			} 
			catch (PartInitException e) 
			{
				//TODO: show a message dialog with teh error 
				e.printStackTrace();
			}
		}		
		return view;
	}
	
	/**
	 * Whenever a new DB opened reset all views. 
	 */
	public static void resetAllViewsToInitialState()
	{ 		
		IWorkbenchPage page = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getActivePage();
		
		IViewPart part =  ViewerManager.getView(page, OMPlusConstants.CLASS_VIEWER_ID);
		if(part instanceof ClassViewer)
		{
			((ClassViewer)part).refreshClassViewerWithNewDB();
		}
		
		part =  ViewerManager.getView(page, OMPlusConstants.QUERY_BUILDER_ID);
		if(part instanceof QueryBuilder)
		{
			((QueryBuilder)part).resetQueryBuilderForNewDB();
		}
		
		part =  ViewerManager.getView(page, OMPlusConstants.PROPERTY_VIEWER_ID);
		if(part instanceof PropertyViewer)
		{
			((PropertyViewer)part).newDBUpdate();
		}
		
		part =  ViewerManager.getView(page, OMPlusConstants.QUERY_RESULTS_ID);
		if(part instanceof QueryResults)
		{
			((QueryResults)part).resetQueryResultsForNewDB();
		}
		
		//explicitly hide QueryResultd view
		hideView(page, OMPlusConstants.QUERY_RESULTS_ID);		
	}
	
	
	/**
	 * The class viewer has been activated
	 */
	public static void classViewActivatetd()
	{
		//TODO: Why QueruGroups not resetted to initial state???
		//Leads to recursion. 
		//If you have dragged something to QueryBuilder and then start input its value
		//Now the QueryBuilder is activated . When you try dragging another item from Class
		//ClassViewer, it gets activated and QueryBuilder restet to start stat...which is not needed
				
		/*IWorkbenchPage page = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getActivePage();
		IViewPart part =  ViewerManager.getView(page, OMPlusConstants.CLASS_VIEWER_ID);
		if(part instanceof ClassViewer)
		{
			((ClassViewer)part).activated();
		}*/
		
		//As of now when class viewer updated just updated property viewer to remove Object propertiies if any
		IWorkbenchPage page = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getActivePage();
		IViewPart part =  ViewerManager.getView(page, OMPlusConstants.CLASS_VIEWER_ID);
		if(part instanceof ClassViewer)
		{
			((ClassViewer)part).activated();
		}
		
	}
	
	
	//TODO: not getting called see Activator.java 
	//Leads to recursion when RunQuery btn fired. QueryBuilder getting
	//updated when Query result is still being updated
	/**
	 * Queryresults view activated
	 */
	public static void queryResultsViewActivatetd()
	{
		//The QueryResults tab has been activated.		
		IWorkbenchPage page = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getActivePage();
		IViewPart part =  ViewerManager.getView(page, OMPlusConstants.QUERY_RESULTS_ID);
		if(part instanceof QueryResults)
		{
			((QueryResults)part).activated();
		}
	}

}
