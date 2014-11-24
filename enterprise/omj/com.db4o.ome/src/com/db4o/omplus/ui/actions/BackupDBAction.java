package com.db4o.omplus.ui.actions;

import java.io.*;

import org.eclipse.jface.action.*;
import org.eclipse.jface.dialogs.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.ui.*;

public class BackupDBAction implements IWorkbenchWindowActionDelegate, IActionDelegate2 {

	private final String BACKUP_REMOTE_MESSAGE = "Backup not possible for remote connection";
	private final String BACKUP_REPLACE_MESSAGE = "Replace the existing file ?";
	private final String BACKUP_SUCCESS_MESSAGE = "Database Backup was successful";
	
	private IWorkbenchWindow window;
	private DatabaseModelListener listener = null;
	
	public void dispose() {
		if(listener != null) {
			Activator.getDefault().dbModel().unregisterListener(listener);
		}
	}

	public void init(IWorkbenchWindow window) {
		this.window = window;
	}
	
	public void init(final IAction action) {
		DatabaseModel model = Activator.getDefault().dbModel();
		action.setEnabled(model.connected());
		listener = new DatabaseModelListener() {
			public void connectedStatusChangedTo(boolean connected) {
				action.setEnabled(connected);
			}
		};
		model.registerListener(listener);
	}

	public void runWithEvent(IAction action, Event event) {
		run(action);
	}
	
	public void run(IAction action) {
		boolean backupComplete = true;
		DbMaintenance main = new DbMaintenance();
		String backupFile = null;
		if(main.isDBOpened()){
			if(main.isClient())
				showMessage();
			else{
				do{
					FileDialog fDialog = new FileDialog(window.getShell(), SWT.SAVE);
					backupFile = fDialog.open();
				}while( backupFile!= null && !isFileExists(backupFile));
				
				if(backupFile != null){
					try{ 
						main.backup(backupFile);
					}
					catch(Exception ex){
						backupComplete = false;
						showErrorMessageDialog("Error creating backup to " + backupFile, ex);
					}
					if(backupComplete)
						showInfoDialog();
				}
			}
		}
	}

	private void showMessage() {
		MessageDialog.openInformation(window.getShell(), OMPlusConstants.DIALOG_BOX_TITLE, 
				BACKUP_REMOTE_MESSAGE);		
	}

	private boolean isFileExists(String backupFile) {
		boolean replace = true;
		if(new File(backupFile).exists()){
			replace = MessageDialog.openQuestion(window.getShell(), OMPlusConstants.DIALOG_BOX_TITLE, 
					BACKUP_REPLACE_MESSAGE);
		}
		return replace;
	}

	private void showInfoDialog() {
		MessageDialog.openInformation(window.getShell(), OMPlusConstants.DIALOG_BOX_TITLE, 
				BACKUP_SUCCESS_MESSAGE);
	}

	private void showErrorMessageDialog(String msg, Exception exc) {
		new ShellErrorMessageSink(window.getShell()).showExc(msg, exc);
	}
	
	public void selectionChanged(IAction actn, ISelection selection) {
	}
}
