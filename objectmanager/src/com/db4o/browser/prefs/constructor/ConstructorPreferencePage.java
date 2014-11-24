/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.constructor;

import java.util.Iterator;

import org.eclipse.jface.preference.PreferencePage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.List;
import org.eclipse.swt.widgets.MessageBox;
import org.eclipse.swt.widgets.Text;

import com.db4o.objectmanager.model.BrowserCore;
import com.db4o.browser.prefs.PreferencesCore;
import com.swtworkbench.community.xswt.XSWT;
import com.swtworkbench.community.xswt.metalogger.Logger;

public class ConstructorPreferencePage extends PreferencePage {
    
    private IConstructorPreferencePagePanel panel;

    protected Control createContents(Composite parent) {
        try {
            panel = (IConstructorPreferencePagePanel) XSWT.createl(parent,
                    "constructorPreferencePagePanel.xswt", getClass(),
                    IConstructorPreferencePagePanel.class);
            loadList();
            // Add event handlers to the buttons
            panel.getAddButton().addSelectionListener(addButtonListener);
            panel.getRemoveButton().addSelectionListener(removeButtonListener);
            return panel.getControl();
        } catch (RuntimeException e) {
            Logger.log().error(e, "Exception creating preference panel");
            return panel.getControl();
        }
    }

    protected void loadList() {
        // Load the list
        List classpathList = panel.getConstructorClassList();
        classpathList.removeAll();
        
        ConstructorPreferences prefs = ConstructorPreferences.getDefault();
        for (Iterator prefsIter = prefs.iterator(); prefsIter.hasNext();) {
            String entry = (String) prefsIter.next();
            classpathList.add(entry);
        }
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.jface.preference.IPreferencePage#performOk()
     */
    public boolean performOk() {
        PreferencesCore prefs = PreferencesCore.getDefault();

        ConstructorPreferences.getDefault();		// Make sure it exists...
        PreferencesCore.commit();

        BrowserCore.getDefault().updateCallConstructorPrefs();

        return true;
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.jface.preference.IPreferencePage#performCancel()
     */
    public boolean performCancel() {
        PreferencesCore.rollback();
        return true;
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.jface.preference.PreferencePage#performDefaults()
     */
    protected void performDefaults() {
        PreferencesCore prefs = PreferencesCore.getDefault();
        
        ConstructorPreferences classpathPrefs = ConstructorPreferences.getDefault();
        classpathPrefs.resetDefaultValues();
        
        loadList();
    }
    
    private SelectionListener addButtonListener = new SelectionAdapter() {
        public void widgetSelected(SelectionEvent e) {
        	Text newClassName = panel.getConstructorClassInput();
        	String newText = newClassName.getText();
			if (newText.length() < 1) {
        		MessageBox notification = new MessageBox(newClassName.getShell(), SWT.OK);
        		notification.setText("Notification");
        		notification.setMessage("Please enter a fully-qualified class name.");
        		notification.open();
        		return;
        	}
        	
        	ConstructorPreferences.getDefault().add(newText);
        	panel.getConstructorClassList().add(newText);
        	newClassName.setText("");
        }
    };
    
    private SelectionListener removeButtonListener = new SelectionAdapter() {
        public void widgetSelected(SelectionEvent e) {
            String[] selection = panel.getConstructorClassList().getSelection();
            for (int i = 0; i < selection.length; i++) {
                ConstructorPreferences.getDefault().remove(selection[i]);
            }
            loadList();
        }
    };

}
