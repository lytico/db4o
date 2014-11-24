package com.db4o.omplus.prefs;

import org.eclipse.jface.preference.IntegerFieldEditor;
import org.eclipse.jface.preference.PreferencePage;
import org.eclipse.jface.preference.StringFieldEditor;
import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.FormAttachment;
import org.eclipse.swt.layout.FormData;
import org.eclipse.swt.layout.FormLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Group;
import org.eclipse.ui.IWorkbench;
import org.eclipse.ui.IWorkbenchPreferencePage;

import com.db4o.omplus.Activator;

@SuppressWarnings("unused")
public class OMEPreferencePage extends PreferencePage implements
		IWorkbenchPreferencePage {
	
	private final int WIDTH = 5;
	private final String ADDRESS_LABEL = "Address: ";
	private final String PORT_LABEL = "Port: ";
	
	private Composite mainComposite;
	private Group groupComposite;
//	private Group pathComposite;

	private StringFieldEditor addressField;
	private IntegerFieldEditor portField;
//	private PathEditor pathEditor;

	public OMEPreferencePage() {
		super();
		// Set the preference store for the preference page.
		setPreferenceStore(Activator.getDefault().getPreferenceStore());
		setDescription("OME Settings");
	}

	protected Control createContents(Composite parent) {
		mainComposite = new Composite(parent, SWT.NONE);
		mainComposite.setLayout(new FormLayout());
				
		groupComposite = new Group(mainComposite, SWT.SHADOW_NONE);
		groupComposite.setText("Proxy Server");
		
//		pathComposite = new Group(mainComposite, SWT.SHADOW_NONE);
//		pathComposite.setText("Configure Classpath");
		setLayoutForMain();

		BlankFieldEditor spc1 = new BlankFieldEditor(groupComposite);
		BlankFieldEditor spc2 = new BlankFieldEditor(groupComposite);
		addressField = new StringFieldEditor(PreferenceConstants.ADDRESS, ADDRESS_LABEL, 50,
				groupComposite);
		addressField.setPage(this);
		addressField.setPreferenceStore(getPreferenceStore());
		addressField.load();
		
		BlankFieldEditor spc3 = new BlankFieldEditor(groupComposite);
		BlankFieldEditor spc4 = new BlankFieldEditor(groupComposite);
		portField = new IntegerFieldEditor(PreferenceConstants.PORT, PORT_LABEL, groupComposite, 5);
		portField.setPage(this);
		portField.setPreferenceStore(getPreferenceStore());
		portField.load();
		
		
/*		pathEditor = new PathEditor(PreferenceConstants.CLASSPATH,"Not Working","Add Folder to classpath", pathComposite);
		pathEditor.setPage(this);
		pathEditor.setPreferenceStore(getPreferenceStore());
		pathEditor.load();*/
		
		return mainComposite;
	}

	private void setLayoutForMain() {
		FormData data = new FormData();
		data.top = new FormAttachment(0, WIDTH);
		data.left = new FormAttachment(0, WIDTH);
		data.right = new FormAttachment(100, -WIDTH);
		data.bottom = new FormAttachment(35);
		groupComposite.setLayoutData(data);
		
	/*	data = new FormData();
		data.top = new FormAttachment(groupComposite,(2 * WIDTH ));
		data.left = new FormAttachment(0, WIDTH);
		data.right = new FormAttachment(100, -WIDTH);
		data.bottom = new FormAttachment(100, -WIDTH);
//		pathComposite.setLayoutData(data);	
*/	}

	public void init(IWorkbench workbench) {
		// TODO Auto-generated method stub

	}

	/*
	 * The user has pressed "Restore defaults".
	 * Restore all default preferences.
	 */
	protected void performDefaults() {
		addressField.loadDefault();
		portField.loadDefault();
//		pathEditor.loadDefault();
		super.performDefaults();
	}
	
	/*
	 * The user has pressed Ok or Apply. Store/apply 
	 * this page's values appropriately.
	 */	
	public boolean performOk() {
		addressField.store();
		portField.store();
//		pathEditor.store();

		return super.performOk();
	}
}
