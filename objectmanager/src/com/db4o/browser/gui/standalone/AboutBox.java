/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.standalone;

import org.eclipse.jface.dialogs.Dialog;
import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CLabel;
import org.eclipse.swt.events.ShellAdapter;
import org.eclipse.swt.events.ShellEvent;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Shell;

import com.db4o.Db4o;

public class AboutBox extends Dialog {

    protected AboutBox(Shell parentShell) {
        super(parentShell);
    }
    
    private Image logo;
    
    protected Control createDialogArea(Composite parent) {
        Composite contents = (Composite) super.createDialogArea(parent);
        contents.setLayout(new GridLayout());
        logo = new Image(Display.getDefault(), AboutBox.class.getResourceAsStream("db4ologo.gif"));
        CLabel logoLabel = new CLabel(contents, SWT.NULL);
        logoLabel.setImage(logo);
        logoLabel.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL | GridData.HORIZONTAL_ALIGN_CENTER));
        Label copyright = new Label(contents, SWT.NULL);
        copyright.setText("Copyright \u00A9 2005 by db4objects, Inc.");
        copyright.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL | GridData.HORIZONTAL_ALIGN_CENTER));
        Label version = new Label(contents, SWT.NULL);
        version.setText(StandaloneBrowser.APPNAME + " " + StandaloneBrowser.VERSION);
        version.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL | GridData.HORIZONTAL_ALIGN_CENTER));
        Label db4oVersion = new Label(contents, SWT.NULL);
        db4oVersion.setText(Db4o.version());
        db4oVersion.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL | GridData.HORIZONTAL_ALIGN_CENTER));
        return contents;
    }
    
    protected void configureShell(Shell newShell) {
        super.configureShell(newShell);
        newShell.setText("About " + StandaloneBrowser.APPNAME);
        newShell.addShellListener(new ShellAdapter() {
            public void shellClosed(ShellEvent e) {
                super.shellClosed(e);
                logo.dispose();
            }
        });
    }

}
