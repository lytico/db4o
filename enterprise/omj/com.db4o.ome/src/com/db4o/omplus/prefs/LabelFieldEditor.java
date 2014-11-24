package com.db4o.omplus.prefs;


import org.eclipse.jface.preference.FieldEditor;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Label;

/**
 * A field editor for displaying labels
 */
class LabelFieldEditor extends FieldEditor {

	private Label label;

	public LabelFieldEditor(String value, Composite parent) {
		super("label", value, parent);
	}

	protected void adjustForNumColumns(int numColumns) {
		((GridData) label.getLayoutData()).horizontalSpan = numColumns;
	}

	protected void doFillIntoGrid(Composite parent, int numColumns) {
		label = getLabelControl(parent);
		GridData gridData = new GridData();
		gridData.horizontalSpan = numColumns;
		gridData.verticalAlignment = GridData.CENTER;
		gridData.grabExcessVerticalSpace = false;
		gridData.horizontalAlignment = GridData.FILL;
		gridData.grabExcessHorizontalSpace = false;
		label.setLayoutData(gridData);
	}

	public int getNumberOfControls() {
		return 1;
	}

	protected void doLoad() {
	}
	protected void doLoadDefault() {
	}
	protected void doStore() {
	}
}
