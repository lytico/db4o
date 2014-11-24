package com.db4o.omplus;

import java.io.*;

import org.eclipse.jface.resource.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;
import org.eclipse.ui.plugin.*;
import org.osgi.framework.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.model.*;

public class Activator extends AbstractUIPlugin {

	public static final String PLUGIN_ID = OMPlusConstants.PLUGIN_ID;

	private final static String CONFIG_DB_PATH_PROPERTY = "com.db4o.ome.configdb.path";
	private final static String CLEAN_CONFIG_DB_PROPERTY = "com.db4o.ome.clean.configdb";
	
	private final static String USR_HOME_DIR_PROPERTY = "user.home";
	private final static String OME_DATA_DB = "OMEDATA.db4o";
	
	private static Activator plugin;

	public static Activator getDefault() {
		return plugin;
	}

	public static ImageDescriptor getImageDescriptor(String path) {
		return imageDescriptorFromPlugin(PLUGIN_ID, path);
	}

	private OMEDataStore dataStore = null;
	private QueryPresentationModel queryModel = null;
	private DatabaseModel dbModel;
	
	public void start(BundleContext context) throws Exception 
	{
		super.start(context);
		
		plugin = this;
		
		//OMEDebugItem.createDebugDatabase();
		
		dbModel = new DatabaseModel();
		
		Display.getDefault().asyncExec(new Runnable() 
		{
		    public void run() 
		    {
		    	PlatformUI.getWorkbench().getActiveWorkbenchWindow().
		    				getActivePage().addPartListener(new IPartListener2() 
		    				{
		    					/**
		    					 * Handle part activated
		    					 */
								public void partActivated(
										IWorkbenchPartReference partRef) 
								{
												    				
									if(partRef.getId().equals(OMPlusConstants.CLASS_VIEWER_ID))
									{
										//TODO: Leads to recursion when RunQuery btn fired. 
										//If you have dragged something to QueryBuilder and then start input its value
										//Now the QueryBuilder is activated . When you try dragging another item from Class
										//ClassViewer, it gets activated and QueryBuilder restet to start stat...which is not needed
										
										ViewerManager.classViewActivatetd();
										
										
									}
									else if(partRef.getId().equals(OMPlusConstants.QUERY_BUILDER_ID))
									{
										//System.out.println("querybuilder activated");
										//ViewerManager.queryResultsViewActivatetd();
										
									}
									else if(partRef.getId().equals(OMPlusConstants.QUERY_RESULTS_ID))
									{
										//TODO: Leads to recursion when RunQuery btn fired. QueryBuilder getting
										//updated when Query result is still being updated
										
										//ViewerManager.queryResultsViewActivatetd();
									}
									else if(partRef.getId().equals("org.eclipse.ui.browser.editor"))
									{
										/*System.out.println("Browser accesed");
										IEditorReference[] i = PlatformUI.getWorkbench().getActiveWorkbenchWindow().
												getActivePage().getEditorReferences();
										if(i.length==0)
										{
											System.out.println("No editor refrences");
										}*/
										
									}
									else
									{
										//System.out.println("NO idea what is activated...no part in OME");
									}
									
									
								}

								public void partBroughtToTop(IWorkbenchPartReference partRef) {
									//  Auto-generated method stub
									
								}

								/**
								 * Handle part closed
								 */
								public void partClosed(	IWorkbenchPartReference partRef) 
								{
									if(partRef.getId().equals(OMPlusConstants.BROWSER_EDITOR_ID))
									{
										BrowserEditorManager.closeEditorAreaIfNoEditors();										
									}
									else
									{
										//System.out.println("What part is closed????????????");
									}
								}

								public void partDeactivated(
										IWorkbenchPartReference partRef) {
								}

								public void partHidden(
										IWorkbenchPartReference partRef) {

								}

								public void partInputChanged(
										IWorkbenchPartReference partRef) 
								{

								}

								public void partOpened(
										IWorkbenchPartReference partRef) {
								}

								public void partVisible(
										IWorkbenchPartReference partRef) {

								}
		    					
		    				});
		    	queryModel = new QueryPresentationModel(new ErrorMessageHandler(new ShellErrorMessageSink(PlatformUI.getWorkbench().getActiveWorkbenchWindow().getShell())));
		    }
		
		});
		
	}

	public void stop(BundleContext context) throws Exception {
		if(dataStore != null) {
			dataStore.close();
		}
		ConnectionStatus status = new ConnectionStatus();
		if(status.isConnected()){
			status.closeExistingDB();
		}
		super.stop(context);
		plugin = null;
	}
	
	public OMEDataStore getOMEDataStore() {
		if(dataStore != null) {
			return dataStore;
		}
		dataStore = new Db4oOMEDataStore(settingsFile(), new DatabasePathPrefixProvider());
		return dataStore;
	}
	
	private String settingsFile() {
		String path = System.getProperty(CONFIG_DB_PATH_PROPERTY);
		if(path == null || path.length() == 0) {
			path = new File(new File(System.getProperty(USR_HOME_DIR_PROPERTY)), OME_DATA_DB).getAbsolutePath();
		}
		boolean doClean = Boolean.valueOf(System.getProperty(CLEAN_CONFIG_DB_PROPERTY, "false"));
		if(doClean) {
			new File(path).delete();
		}
		return path;
	}

	public DatabaseModel dbModel() {
		return dbModel;
	}
	
	public QueryPresentationModel queryModel() {
		return queryModel;
	}
	
	private static class DatabasePathPrefixProvider implements ContextPrefixProvider {
		public String currentPrefix() {
			DatabaseModel dbModel = Activator.getDefault().dbModel();
			return dbModel.connected() ? dbModel.db().getDbPath() : "";
		}
	}

}
