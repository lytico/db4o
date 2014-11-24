package com.db4o.omplus.ui;

import org.eclipse.ui.IEditorReference;
import org.eclipse.ui.IWorkbench;
import org.eclipse.ui.IWorkbenchPage;
import org.eclipse.ui.IWorkbenchWindow;
import org.eclipse.ui.PlatformUI;

/**
 * Class that simple deals with handling the editor area currently used for showinbg the
 * browsers
 * 
 * @author prameela_nair
 *
 */
public class BrowserEditorManager 
{
	/**
	 * Close teh editor area if no editors present
	 */
	public static void closeEditorAreaIfNoEditors()
	{
		
		//LOGIC: use the the null checks. This function called my the ClosedListener even the workbench has 
		//abruptly closed and so individual entires may have become null
		
		IWorkbench workbench = PlatformUI.getWorkbench();
		if(workbench == null)
		{
			System.out.println("Workbench returned null. Returning w/o handling partClosed ");
			return;
		}
		IWorkbenchWindow workbenchWindow = workbench.getActiveWorkbenchWindow();
		if(workbenchWindow == null)
		{
			System.out.println("WorkbenchWindow returned as null. Returning w/o handling partClosed ");
			return;
		}
		IWorkbenchPage workbenchPage = workbenchWindow.getActivePage();
		if(workbenchPage == null)
		{
			System.out.println("WorkbenchPage returned as null. Returning w/o handling partClosed ");
			return;
		}
		IEditorReference[] refs = workbenchPage.getEditorReferences();
		
									
		if(refs == null || refs.length==0)
		{
			//set the editor area to invisible
			PlatformUI.getWorkbench().getActiveWorkbenchWindow().
			getActivePage().setEditorAreaVisible(false);
		}
	}

}
