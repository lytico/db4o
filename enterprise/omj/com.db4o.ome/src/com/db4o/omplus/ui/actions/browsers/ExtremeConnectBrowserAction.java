package com.db4o.omplus.ui.actions.browsers;

import java.net.MalformedURLException;
import java.net.URL;

import org.eclipse.jface.action.IAction;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.ui.IEditorPart;
import org.eclipse.ui.IEditorReference;
import org.eclipse.ui.IWorkbench;
import org.eclipse.ui.IWorkbenchPage;
import org.eclipse.ui.IWorkbenchWindow;
import org.eclipse.ui.IWorkbenchWindowActionDelegate;
import org.eclipse.ui.PartInitException;
import org.eclipse.ui.PlatformUI;
import org.eclipse.ui.browser.IWebBrowser;
import org.eclipse.ui.browser.IWorkbenchBrowserSupport;

import com.db4o.omplus.datalayer.OMPlusConstants;

public class ExtremeConnectBrowserAction implements IWorkbenchWindowActionDelegate {

	public void dispose() 
	{
		// Auto-generated method stub

	}

	public void init(IWorkbenchWindow window) 
	{
		// Auto-generated method stub

	}

	public void run(IAction action) 
	{
		//MessageDialog.openInformation(null, "!!!", "Exterme connect - google");
//		single multiline comment for avoiding web service
/*		if(!PermissionValidator.checkIfUserHasLoggedIntoWebService())
		{
			WebServiceLoginDialog dialog = PermissionValidator.showWebServiceLoginDialog(OMPlusConstants.WEB_SERVICE_PAIRING);
			Shell dialogShell = dialog.getShell();
			if(dialogShell != null)
			{
				Shell mainShell = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getShell();
				Display display = mainShell.getDisplay();
				while (!dialogShell.isDisposed()) 
			    {
			      if (!display.readAndDispatch())
			    	  display.sleep();
			    }
			}
			
			//If cancel clicked ont the dialog return  
			if(dialog.getButtonClicked() == OMPlusConstants.DIALOG_CANCEL_CLICKED)
			{
				return;
			}
			
			WebServiceLoginDialog dialog = PermissionValidator.showWebServiceLoginDialog(OMPlusConstants.WEB_SERVICE_PAIRING);
			Shell dialogShell = null;
			
			if(dialog != null)
			{
				dialogShell = dialog.getShell();				
			
				//Go into sleep mode till teh dialog box not disposed
				if(dialogShell != null)
				{
					Shell mainShell = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getShell();
					Display display = mainShell.getDisplay();
					while (!dialogShell.isDisposed()) 
				    {
				      if (!display.readAndDispatch())
				    	  display.sleep();
				    }
				}
				
				//If cancel clicked ont the dialog return  
				if(dialog.getButtonClicked() == OMPlusConstants.DIALOG_CANCEL_CLICKED)
				{
					return;
				}
			}
			
		}		
		
		if(!PermissionValidator.checkIfUserHasPermissionForService(OMPlusConstants.WEB_SERVICE_PAIRING))
		{
			MessageDialog.openInformation(null, OMPlusConstants.DIALOG_BOX_TITLE,
										"You do not have permissions to access this service");
			return;
		}*/ 
		
		IWorkbench workbench = PlatformUI.getWorkbench();
		
		
		IWorkbenchBrowserSupport support =	workbench.getBrowserSupport();
		try 
		{
			IEditorReference ref[]  =workbench.getActiveWorkbenchWindow().
					getActivePage().getEditorReferences();

			for (int i = 0; i < ref.length; i++) 
			{
				//System.out.println("Browser NO "+i+"= "+ref[i].getTitle());
				if(ref[i].getTitle().equals(OMPlusConstants.EXTREME_CONNECT_BROWSER_NAME))
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
		    									OMPlusConstants.EXTREME_CONNECT_BROWSER_ID,
		    									OMPlusConstants.EXTREME_CONNECT_BROWSER_NAME,
		    									OMPlusConstants.EXTREME_CONNECT_TOOLTIP);
			browser.openURL(new URL(OMPlusConstants.EXTREME_CONNECT_URL));
			
			//To maximize teh browser state.
			//TODO: check if this can directly be done using some style settings
			ref  =workbench.getActiveWorkbenchWindow().
			getActivePage().getEditorReferences();

			for (int i = 0; i < ref.length; i++) 
			{
				//System.out.println("Browser NO "+i+"= "+ref[i].getTitle());
				if(ref[i].getTitle().equals(OMPlusConstants.EXTREME_CONNECT_BROWSER_NAME))
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

	public void selectionChanged(IAction action, ISelection selection) 
	{
		// Auto-generated method stub

	}

}
