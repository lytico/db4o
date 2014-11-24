/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.activation;

import org.eclipse.jface.preference.PreferencePage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;

import com.db4o.browser.prefs.PreferencesCore;

/**
 * ActivationPreferencePage.
 *
 * @author djo
 */
public class ActivationPreferencePage extends PreferencePage {

	ActivationPreferencePagePanel panel;
	
	/* (non-Javadoc)
	 * @see org.eclipse.jface.preference.PreferencePage#createContents(org.eclipse.swt.widgets.Composite)
	 */
	protected Control createContents(Composite parent) {
		panel = new ActivationPreferencePagePanel(parent, SWT.NULL);
		
		ActivationPreferences actPrefs = ActivationPreferences.getDefault();
		panel.getInitialActivationDepth().setSelection(actPrefs.getInitialActivationDepth());
		panel.getSubsequentActivationDepth().setSelection(actPrefs.getSubsequentActivationDepth());
		
		return panel;
	}
	
	/* (non-Javadoc)
	 * @see org.eclipse.jface.preference.IPreferencePage#performOk()
	 */
	public boolean performOk() {
		PreferencesCore prefs = PreferencesCore.getDefault();
		
		// Global activation depth handling...
		ActivationPreferences actPrefs = ActivationPreferences.getDefault();
		actPrefs.setInitialActivationDepth(panel.getInitialActivationDepth().getSelection());
		actPrefs.setSubsequentActivationDepth(panel.getSubsequentActivationDepth().getSelection());
		
		PreferencesCore.commit();
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
		
		ActivationPreferences actPrefs = ActivationPreferences.getDefault();
		actPrefs.resetDefaultValues();

		panel.getInitialActivationDepth().setSelection(actPrefs.getInitialActivationDepth());
		panel.getSubsequentActivationDepth().setSelection(actPrefs.getSubsequentActivationDepth());
	}
	

}
