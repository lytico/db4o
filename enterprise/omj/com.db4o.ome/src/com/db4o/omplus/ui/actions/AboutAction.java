package com.db4o.omplus.ui.actions;

import org.eclipse.jface.action.IAction;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.ui.IWorkbenchWindow;
import org.eclipse.ui.IWorkbenchWindowActionDelegate;

import com.db4o.omplus.ui.dialog.AboutDialog;

public class AboutAction implements IWorkbenchWindowActionDelegate 
{
	private IWorkbenchWindow window;
	public void dispose() {
		//  Auto-generated method stub
		
	}

	public void init(IWorkbenchWindow window)
	{
		this.window = window;
		
	}

	public void run(IAction action) 
	{
	
		 //MessageDialog.openInformation(null, "", "Add about dialog");
		AboutDialog aboutDialog = new AboutDialog(window.getShell());
		aboutDialog.open();
		

		
	}

	public void selectionChanged(IAction action, ISelection selection) {
		//  Auto-generated method stub
		
	}

}
