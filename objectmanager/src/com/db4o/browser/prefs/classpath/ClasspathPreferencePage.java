/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.classpath;

import java.util.Iterator;

import org.eclipse.jface.preference.PreferencePage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.DirectoryDialog;
import org.eclipse.swt.widgets.FileDialog;
import org.eclipse.swt.widgets.List;

import com.db4o.objectmanager.model.BrowserCore;
import com.db4o.browser.prefs.PreferencesCore;
import com.swtworkbench.community.xswt.XSWT;
import com.swtworkbench.community.xswt.metalogger.Logger;

public class ClasspathPreferencePage extends PreferencePage {
    
    private IClasspathPreferencePagePanel panel;

    protected Control createContents(Composite parent) {
        try {
            panel = (IClasspathPreferencePagePanel) XSWT.createl(parent,
                    "classpathPreferencePagePanel.xswt", getClass(),
                    IClasspathPreferencePagePanel.class);
            loadList();
            // Add event handlers to the buttons
            panel.getAddDirectoryButton().addSelectionListener(directoryButtonListener);
            panel.getAddJarZipButton().addSelectionListener(jarzipButtonListener);
            panel.getRemoveButton().addSelectionListener(removeButtonListener);
            return panel.getControl();
        } catch (RuntimeException e) {
            Logger.log().error(e, "Exception creating preference panel");
            return panel.getControl();
        }
    }

    protected void loadList() {
        // Load the list
        List classpathList = panel.getClasspathList();
        classpathList.removeAll();
        
        ClasspathPreferences prefs = ClasspathPreferences.getDefault();
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

        ClasspathPreferences classpathPrefs = ClasspathPreferences.getDefault();
        PreferencesCore.commit();

        BrowserCore.getDefault().updateClasspath();

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
        
        ClasspathPreferences classpathPrefs = ClasspathPreferences.getDefault();
        classpathPrefs.resetDefaultValues();
        
        loadList();

//        panel.getInitialActivationDepth().setSelection(actPrefs.getInitialActivationDepth());
//        panel.getSubsequentActivationDepth().setSelection(actPrefs.getSubsequentActivationDepth());
    }
    
    private SelectionListener directoryButtonListener = new SelectionAdapter() {
        public void widgetSelected(SelectionEvent e) {
            DirectoryDialog dialog = new DirectoryDialog(panel.getControl().getShell(), SWT.OPEN);
            String file = dialog.open();
            if (file != null) {
                ClasspathPreferences classpathPrefs = ClasspathPreferences.getDefault();
                classpathPrefs.add(file);
                
                panel.getClasspathList().add(file);
//                browserController.addToClasspath(new File(file));
            }
        }
    };
    
    private SelectionListener jarzipButtonListener = new SelectionAdapter() {
        public void widgetSelected(SelectionEvent e) {
            FileDialog dialog = new FileDialog(panel.getControl().getShell(), SWT.OPEN);
            dialog.setFilterExtensions(new String[]{"*.jar","*.zip"});
            String file = dialog.open();
            if (file != null) {
                ClasspathPreferences classpathPrefs = ClasspathPreferences.getDefault();
                classpathPrefs.add(file);
                
                panel.getClasspathList().add(file);
//                browserController.addToClasspath(new File(file));
            }
        }
    };
    
    private SelectionListener removeButtonListener = new SelectionAdapter() {
        public void widgetSelected(SelectionEvent e) {
            String[] selection = panel.getClasspathList().getSelection();
            for (int i = 0; i < selection.length; i++) {
                ClasspathPreferences.getDefault().remove(selection[i]);
            }
            loadList();
        }
    };

}
