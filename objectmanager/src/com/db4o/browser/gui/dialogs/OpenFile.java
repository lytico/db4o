/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.dialogs;

import java.io.File;

import org.eclipse.jface.dialogs.Dialog;
import org.eclipse.jface.dialogs.IDialogConstants;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.events.VerifyEvent;
import org.eclipse.swt.events.VerifyListener;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.FileDialog;
import org.eclipse.swt.widgets.Shell;
import org.eclipse.ve.sweet.validator.IValidator;

import com.swtworkbench.community.xswt.XSWT;

public class OpenFile extends Dialog {

    public OpenFile(Shell shell) {
        super(shell);
        setBlockOnOpen(true);
    }
    
    private IOpenFile pane;

    protected Control createDialogArea(Composite composite) {
        Composite holder = (Composite) super.createDialogArea(composite);
        
        pane = (IOpenFile) XSWT.createl(holder,
                "openFile.xswt", getClass(), IOpenFile.class);
        
        pane.getFileName().addVerifyListener(verifyFileName);
        pane.getBrowseButton().addSelectionListener(browseForFile);

        return holder;
    }
    
    protected void createButtonsForButtonBar(Composite arg0) {
        super.createButtonsForButtonBar(arg0);
        getButton(IDialogConstants.OK_ID).setEnabled(false);
    }
    
    protected void configureShell(Shell shell) {
        super.configureShell(shell);
        shell.setText("Open \"encrypted\" file");
    }
    
    private IValidator fileNameVerifier = new IValidator() {
        public String isValidPartialInput(String fragment) {
            return null;
        }

        public String isValid(Object value) {
            File file = new File((String)value);
            if (file.isFile()) {
                return null;
            }
            return getHint();
        }
        
        public String getHint() {
            return "Please enter a legal path and file name";
        }

    };
    
    private VerifyListener verifyFileName = new VerifyListener() {
        public void verifyText(VerifyEvent e) {
            String currentText = pane.getFileName().getText();
            String newValue = currentText.substring(0, e.start) + e.text + currentText.substring(e.end);
            if (fileNameVerifier.isValidPartialInput(newValue) != null) {
                e.doit = false;
                verifyEverything(currentText);
            } else {
                verifyEverything(newValue);
            }
        }
    };

    protected void verifyEverything(String fileName) {
        String error = fileNameVerifier.isValid(fileName);
        if (error == null) {
            getButton(IDialogConstants.OK_ID).setEnabled(true);
            pane.getHelpArea().setText("");
        } else {
            getButton(IDialogConstants.OK_ID).setEnabled(false);
            pane.getHelpArea().setText(error);
        }
    }
    
    
    protected SelectionListener browseForFile = new SelectionAdapter() {
        public void widgetSelected(SelectionEvent e) {
            FileDialog dialog = new FileDialog(pane.getBrowseButton().getShell(), SWT.OPEN);
            dialog.setFilterExtensions(new String[]{"*.yap", "*"});
            String file = dialog.open();
            if (file != null) {
                pane.getFileName().setText(file);
                verifyEverything(file);
            }
        }
    };
    
    private String fileName = "";
    private String password = "";
    private boolean readOnly=false;
    
    protected void okPressed() {
        fileName = pane.getFileName().getText();
        password = pane.getPassword().getText();
        readOnly=pane.getReadOnly().getSelection();
        super.okPressed();
    }

    /**
     * @return Returns the password.
     */
    public String getPassword() {
        return password;
    }

    /**
     * @return Returns the fileName.
     */
    public String getFileName() {
        return fileName;
    }
    
    public boolean getReadOnly() {
    	return readOnly;
    }
}
