package com.db4o.omplus.ui.actions.browsers;

import java.io.File;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.StringTokenizer;

import org.eclipse.core.runtime.FileLocator;
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

public class HelpBrowserAction implements IWorkbenchWindowActionDelegate 
{

	public void dispose() {
		// Auto-generated method stub
		
	}

	public void init(IWorkbenchWindow window) {
		// Auto-generated method stub
		
	}

	public void run(IAction action)
	{
		//MessageDialog.openInformation(null, "!!!", "Help - rediff");
		
		IWorkbench workbench = PlatformUI.getWorkbench();
		IWorkbenchBrowserSupport support =	workbench.getBrowserSupport();
		try 
		{
			IEditorReference ref[]  =workbench.getActiveWorkbenchWindow().
					getActivePage().getEditorReferences();

			for (int i = 0; i < ref.length; i++) 
			{
				//System.out.println("Browser NO "+i+"= "+ref[i].getTitle());
				if(ref[i].getTitle().equals(OMPlusConstants.HELP_BROWSER_NAME))
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
		    									OMPlusConstants.HELP_BROWSER_ID,
		    									OMPlusConstants.HELP_BROWSER_NAME,
		    									OMPlusConstants.HELP_BROWSER_TOOLTIP);
			
//			URL url = Platform.getBundle(OMPlusConstants.PLUGIN_ID).getEntry(OMPlusConstants.HELP_BROWSER_LOCATION);
			URL url = getURL();
			browser.openURL(FileLocator.toFileURL(url));
			
			
			//To maximize teh browser state.
			//TODO: check if this can directly be done using some style settings
			ref  =workbench.getActiveWorkbenchWindow().
			getActivePage().getEditorReferences();

			for (int i = 0; i < ref.length; i++) 
			{
				//System.out.println("Browser NO "+i+"= "+ref[i].getTitle());
				if(ref[i].getTitle().equals(OMPlusConstants.HELP_BROWSER_NAME))
				{
					PlatformUI.getWorkbench().getActiveWorkbenchWindow().
												getActivePage().setPartState(ref[i],
														IWorkbenchPage.STATE_MAXIMIZED);
					
					return;
				}
			}
			
		} 
		/*catch (MalformedURLException e) 
		{
			e.printStackTrace();
		}*/
		catch (IOException e) 
		{
			e.printStackTrace();
		} 
		catch (PartInitException e) 
		{
			e.printStackTrace();
		} 
		
		
	}

	private URL getURL() {
		URL url = null;
		String path =  System.getProperty(OMPlusConstants.CLASSPATH);
		String []entries = path.split(";");
		for( String cPath : entries){
			cPath = cPath.replace('\\', OMPlusConstants.BACKSLASH);
			if(!cPath.contains("eclipse/plugins"))
				continue;
			StringBuilder sb = new StringBuilder("file:///");
			sb.append(cPath.split(OMPlusConstants.PLUGIN_FLD)[0]);
			sb.append(OMPlusConstants.PLUGIN_FLD);
			sb.append(OMPlusConstants.BACKSLASH);
			sb.append("com.db4o.ome_1.0.0");sb.append(OMPlusConstants.BACKSLASH);
			if(cPath != null){
				try {
					sb.append(OMPlusConstants.HELP_BROWSER_LOCATION);
					url = new URL(sb.toString());
				} catch (MalformedURLException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
			return url;
		}
		return null;
	}

	public static void printClasspath() {

		System.out.println("\nClasspath:");
		StringTokenizer tokenizer = 
			new StringTokenizer(System.getProperty("java.class.path"), File.pathSeparator);
		while (tokenizer.hasMoreTokens()) {
			System.out.println(tokenizer.nextToken());
		}	
	}


	
	
	public void selectionChanged(IAction action, ISelection selection) {
		// Auto-generated method stub
		
	}


}
