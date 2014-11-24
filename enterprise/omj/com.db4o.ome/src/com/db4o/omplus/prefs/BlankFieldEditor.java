package com.db4o.omplus.prefs;


import org.eclipse.swt.widgets.Composite;

/**
 * A field editor for adding a blank field to a preference page.
 */
public class BlankFieldEditor extends LabelFieldEditor {
	public BlankFieldEditor(Composite parent) {
		super("", parent);
	}
}
