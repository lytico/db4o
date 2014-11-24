package com.db4o.omplus.ui.actions.browsers;

import java.net.*;

import org.eclipse.jface.action.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.ui.*;
import org.eclipse.ui.browser.*;

import com.db4o.omplus.datalayer.*;

public class SupportCasesBrowserAction implements IWorkbenchWindowActionDelegate {

	public void dispose() {
		// Auto-generated method stub

	}

	public void init(IWorkbenchWindow window) {
		// Auto-generated method stub

	}

	public void run(IAction action)
	{
		IWorkbench workbench = PlatformUI.getWorkbench();
		IWorkbenchBrowserSupport support =	workbench.getBrowserSupport();
		try 
		{
			IEditorReference ref[]  =workbench.getActiveWorkbenchWindow().
					getActivePage().getEditorReferences();

			for (int i = 0; i < ref.length; i++) 
			{
				//System.out.println("Browser NO "+i+"= "+ref[i].getTitle());
				if(ref[i].getTitle().equals(OMPlusConstants.SUPPORT_CASES_BROWSER_NAME))
				{
					//System.out.println("Caught you..............." + ref[i].getTitle() );
					IEditorPart part = ref[i].getEditor(false);
					
					PlatformUI.getWorkbench().getActiveWorkbenchWindow().
												getActivePage().activate(part);		
					
					return;
				}
			}
			
			/**
			 * AS_External -> launches ur browser in corr default browser...say Mozilla/IE
			 * AS_VIEW -> launches ur browser in a view
			 * AS_EDITOR -> launches ur browser in the editor area
			 */
			int browserStyle = IWorkbenchBrowserSupport.AS_EDITOR|IWorkbenchBrowserSupport.LOCATION_BAR|
						IWorkbenchBrowserSupport.NAVIGATION_BAR;
			
			IWebBrowser browser = support.createBrowser(browserStyle,
		    									OMPlusConstants.SUPPORT_CASES_BROWSER_ID,
		    									OMPlusConstants.SUPPORT_CASES_BROWSER_NAME,
		    									OMPlusConstants.SUPPORT_CASES_TOOLTIP);
			browser.openURL(new URL(OMPlusConstants.SUPPORT_CASES_URL));
			
			//To maximize teh browser state.
			//TODO: check if this can directly be done using some style settings
			ref  =workbench.getActiveWorkbenchWindow().
			getActivePage().getEditorReferences();

			for (int i = 0; i < ref.length; i++) 
			{
				//System.out.println("Browser NO "+i+"= "+ref[i].getTitle());
				if(ref[i].getTitle().equals(OMPlusConstants.SUPPORT_CASES_BROWSER_NAME))
				{
					PlatformUI.getWorkbench().getActiveWorkbenchWindow().
												getActivePage().setPartState(ref[i],
														IWorkbenchPage.STATE_MAXIMIZED);
					
					return;
				}
			}
		} 
		catch (PartInitException e) 
		{
			e.printStackTrace();
		} 
		catch (MalformedURLException e) 
		{
			e.printStackTrace();
		}

		
	}

	public void selectionChanged(IAction action, ISelection selection) {
		// Auto-generated method stub

	}

}
