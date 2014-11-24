package com.db4o.omplus.ui.actions;

import org.eclipse.jface.action.*;
import org.eclipse.jface.dialogs.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.dialog.login.*;
import com.db4o.omplus.ui.dialog.login.model.*;


public class ConnectToDBAction implements IWorkbenchWindowActionDelegate, IActionDelegate2 {
	
	private IWorkbenchWindow window;
	private DatabaseModelListener listener;
	
	public void init(final IAction action) {
		DatabaseModel model = Activator.getDefault().dbModel();
		listener = new DatabaseModelListener() {
			public void connectedStatusChangedTo(boolean connected) {
				setToolTip(action, connected);
			}
		};
		model.registerListener(listener);
		setToolTip(action, model.connected());
	}

	private void setToolTip(IAction action, boolean connected) {
		action.setToolTipText(connected ? "Disconnect" : "Connect");
	}
	
	public void runWithEvent(IAction action, Event event) {
		run(action);
	}

	public void run(IAction action) {
		ConnectionStatus connStatus = new ConnectionStatus();
		boolean connectionClosed = true;
		if(connStatus.isConnected()) { // if connected to database
			connectionClosed = showMessageForConnClose(connStatus.getCurrentDB());
			if(connectionClosed){
				connStatus.closeExistingDB();
				action.setToolTipText("Connect");
				closeOMEPerspective();// FIX for Close db & refresh views
				showOMEPerspective(); 
			}
		}
		else {
			Connector connector = new Connector() {
				public boolean connect(ConnectionParams params) throws DBConnectException {
					return DbConnectUtil.connect(params, window.getShell());
				}
			};
			DataStoreRecentConnectionList recentConnections = new DataStoreRecentConnectionList(Activator.getDefault().getOMEDataStore());
			ErrorMessageHandler errHandler = new ErrorMessageHandler(new ShellErrorMessageSink(window.getShell()));
			LoginDialog myWindow = new LoginDialog(window.getShell(), recentConnections, connector, errHandler);
			myWindow.open();
		}
	}

	private boolean showMessageForConnClose(String fileName) {
		return MessageDialog.openQuestion(window.getShell(), OMPlusConstants.DIALOG_BOX_TITLE,
									"Close Existing DB Connection "+fileName+ " ?");
	}
	
	private void closeOMEPerspective(){
		IWorkbenchPage page = PlatformUI.getWorkbench().getActiveWorkbenchWindow().getActivePage();
//		IPerspectiveDescriptor pers = page.getPerspective();
//		if(pers.getId().equals(OMPlusConstants.OME_PERSPECTIVE_ID)) FIX for OMJ-61
			page.closePerspective(page.getPerspective(), true, true);
			ViewerManager.resetAllViewsToInitialState();
	}
	
	private void showOMEPerspective() {
		IWorkbench workbench = PlatformUI.getWorkbench();
		try {
			workbench.showPerspective(OMPlusConstants.OME_PERSPECTIVE_ID, workbench.getActiveWorkbenchWindow());
		} catch (WorkbenchException e) {}
	}

	public void selectionChanged(IAction actn, ISelection selection) {
	}

	public void dispose() {
		if(listener != null) {
			Activator.getDefault().dbModel().unregisterListener(listener);
		}
	}

	public void init(IWorkbenchWindow window) {
		this.window = window;
	}

}