package com.db4o.omplus.ui.actions;

import org.eclipse.core.resources.*;
import org.eclipse.core.runtime.*;
import org.eclipse.jface.action.*;
import org.eclipse.jface.dialogs.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.ui.*;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.ui.*;

public abstract class OpenDb4oAction implements IObjectActionDelegate {
	
	IWorkbenchPart targetPart;
	String filePath;
	private final boolean readOnly;

	protected OpenDb4oAction(boolean readOnly) {
		this.readOnly = readOnly;
	}

	public void setActivePart(IAction action, IWorkbenchPart targetPart)
    {
        this.targetPart = targetPart;
    }

    public IWorkbenchPart getTargetPart()
    {
        return targetPart;
    }

	public void run(IAction action) {
//		Need to add in recent Connections.
		StringBuilder fullPath = new StringBuilder(Platform.getLocation().toString());
		fullPath.append(filePath).toString();
		FileConnectionParams params = new FileConnectionParams(fullPath.toString(), readOnly);
		try{
			ConnectionStatus status = new ConnectionStatus();
			if(status.isConnected()){
				boolean open = MessageDialog.openQuestion(null, OMPlusConstants.DIALOG_BOX_TITLE, 
						"Do you want to close the existing db and continue?");
				if(!open)
					return;
				status.closeExistingDB();
			}
			DbConnectUtil.connect(params, getTargetPart().getSite().getShell());
			RecentConnectionList list = new DataStoreRecentConnectionList(Activator.getDefault().getOMEDataStore());
			list.addNewConnection(params);
			showPerspective();
		}
		/*
		catch(ClassCastException ex){
			String msg = ex.getMessage();
			if(msg.equals(GENERIC_OBJ))
				msg = "Couldn't open .NET database in OME eclipse plugin";
			err.error(msg, ex);
		}
		*/
		catch(Exception ex){
			ErrorMessageHandler err = new ErrorMessageHandler(new ShellErrorMessageSink(getTargetPart().getSite().getShell()));
			err.error("Could not open " + fullPath + ": " + ex.getMessage(), ex);
		}
	}
	
	private void  showPerspective() 
	{
		
		try {
			//Show the perspective always else views not arranged as needed
			PlatformUI.getWorkbench().showPerspective(OMPlusPerspective.ID, PlatformUI.getWorkbench().getActiveWorkbenchWindow());
			ViewerManager.resetAllViewsToInitialState();
			
		} catch (WorkbenchException e1) {
			e1.printStackTrace();
		}
		
	}

	public void selectionChanged(IAction action, ISelection selection) {
		 IStructuredSelection sel = (IStructuredSelection)selection;
         Object obj = sel.getFirstElement();
         if(obj instanceof IFile)
        	 filePath = ((IFile)obj).getFullPath().toString();
	}

}
