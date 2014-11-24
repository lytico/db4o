/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.dialogs;

import java.util.Iterator;
import java.util.LinkedList;

import org.eclipse.jface.dialogs.Dialog;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.KeyAdapter;
import org.eclipse.swt.events.KeyEvent;
import org.eclipse.swt.events.MouseAdapter;
import org.eclipse.swt.events.MouseEvent;
import org.eclipse.swt.events.VerifyEvent;
import org.eclipse.swt.events.VerifyListener;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.List;
import org.eclipse.swt.widgets.Shell;
import org.eclipse.swt.widgets.Text;

public class ListSelector extends Dialog {

    private String text="";
    private List list;
    private Text textField; 

    private IListPopulator listPopulator;
    private LinkedList choices = new LinkedList();

    public ListSelector(Shell parentShell) {
        super(parentShell);
    }

    protected Control createDialogArea(Composite parent) {
        Composite container = (Composite) super.createDialogArea(parent);
        Composite dialogArea = new Composite(container, SWT.NULL);
        dialogArea.setLayout(new GridLayout());
        textField = new Text(dialogArea, SWT.BORDER);
        textField.setLayoutData(new GridData(GridData.FILL_HORIZONTAL | GridData.GRAB_HORIZONTAL));
        list = new List(dialogArea, SWT.BORDER | SWT.V_SCROLL);
        GridData gd = new GridData(GridData.FILL_BOTH | GridData.GRAB_HORIZONTAL | GridData.GRAB_VERTICAL);
        gd.heightHint = 500;
        list.setLayoutData(gd);
        listPopulator.populate(choices);
        
        updateList("");
        
        textField.addKeyListener(new KeyAdapter() {
            public void keyPressed(KeyEvent e) {
                if (e.character == '\r' || e.character == '\n') {
                    okPressed();
                }
            }
        });
        textField.addVerifyListener(new VerifyListener() {
            public void verifyText(VerifyEvent e) {
                String currentText = textField.getText();
                String newValue = currentText.substring(0, e.start) + e.text + currentText.substring(e.end);
                updateList(newValue);
            }
        });
        list.addMouseListener(new MouseAdapter() {
            public void mouseDoubleClick(MouseEvent e) {
                okPressed();
            }
        });
        return container;
    }
    
    private String lastTextSelection = null;

    private void updateList(String textSelection) {
        if (textSelection.equals(lastTextSelection)) {
            return;
        }
        
        lastTextSelection = textSelection;
        list.removeAll();
        if (textSelection.equals("")) {
            for (Iterator choicesIter = choices.iterator(); choicesIter.hasNext();) {
                String choice = (String) choicesIter.next();
                list.add(choice);
            }
        } else {
            textSelection = textSelection.toUpperCase();
            for (Iterator choicesIter = choices.iterator(); choicesIter.hasNext();) {
                String choice = (String) choicesIter.next();
                String uppercaseChoice = choice.toUpperCase();
                if (uppercaseChoice.indexOf(textSelection) >= 0) {
                    list.add(choice);
                }
            }
            list.select(0);
        }
    }

    protected void configureShell(Shell newShell) {
        super.configureShell(newShell);
        newShell.setText(text);
    }
    
    protected void okPressed() {
        String listSelection = list.getItem(list.getSelectionIndex());
        int realSelectionIndex=0;
        for (Iterator choicesIter = choices.iterator(); choicesIter.hasNext();) {
            String choice = (String) choicesIter.next();
            if (choice.equals(listSelection)) {
                selection = realSelectionIndex;
                break;
            }
            ++realSelectionIndex;
        }
        super.okPressed();
    }
    
    private int selection = -1;

    /**
     * @return Returns the selection.
     */
    public int getSelection() {
        return selection;
    }

    /**
     * @return Returns the listPopulator.
     */
    public IListPopulator getListPopulator() {
        return listPopulator;
    }
    

    /**
     * @param listPopulator The listPopulator to set.
     */
    public void setListPopulator(IListPopulator listPopulator) {
        this.listPopulator = listPopulator;
    }

    /**
     * @return Returns the text.
     */
    public String getText() {
        return text;
    }
    

    /**
     * @param text The text to set.
     */
    public void setText(String text) {
        this.text = text;
        Shell current = getShell();
        if (current != null && !current.isDisposed()) {
            current.setText(text);
        }
    }
    
}
