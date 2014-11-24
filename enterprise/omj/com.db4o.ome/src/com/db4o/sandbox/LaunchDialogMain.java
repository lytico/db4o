/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.sandbox;

import org.eclipse.jface.dialogs.*;
import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;

public class LaunchDialogMain {

	public static void main(String[] args) {
	    Display display = new Display();
	    final Shell shell = new Shell(display);
	    shell.open();

	    final DialogTray tray = new DialogTray() {
			@Override
			protected Control createContents(Composite parent) {
				Tree tree = new Tree(parent, SWT.NONE);
				TreeItem local = new TreeItem(tree, SWT.NONE);
				local.setText("Local");
				TreeItem localEntry = new TreeItem(local, SWT.NONE);
				localEntry.setText("foo.db4o");
				TreeItem remote = new TreeItem(tree, SWT.NONE);
				remote.setText("Remote");
				return tree;
			}
	    };
	    TrayDialog dialog = new TrayDialog(shell) {
	    	@Override
	    	protected Control createContents(Composite parent) {
	    		openTray(tray);
	    		return super.createContents(parent);
	    	}
	    };
	    dialog.open();
	    while (!shell.isDisposed()) {
	      if (!display.readAndDispatch())
	        display.sleep();
	    }
	    display.dispose();
	}

}
