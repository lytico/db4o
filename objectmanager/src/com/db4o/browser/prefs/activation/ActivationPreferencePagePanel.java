/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.activation;

import java.util.Map;

import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.List;
import org.eclipse.swt.widgets.Spinner;
import org.eclipse.swt.widgets.Text;

import com.swtworkbench.community.xswt.XSWT;

public class ActivationPreferencePagePanel extends Composite {
	private Map contents;

	public ActivationPreferencePagePanel(Composite parent, int style) {
		super(parent, style);
		setLayout(new GridLayout());
		contents = XSWT.createl(this, "ActivationPreferencePagePanel.xswt", getClass());
	}
	
	// Global activation depth settings....
	
	public Spinner getInitialActivationDepth() {
		return (Spinner) contents.get("InitialActivationDepth");
	}
	
	public Spinner getSubsequentActivationDepth() {
		return (Spinner) contents.get("SubsequentActivationDepth");
	}
	
	// Per-class activation depth settings....
	
	public Text getClassName() {
		return (Text) contents.get("ClassName");
	}
	
	public Button getBrowseClasses() {
		return (Button) contents.get("BrowseClasses");
	}
	
	public Spinner getClassActivationDepth() {
		return (Spinner) contents.get("ClassActivationDepth");
	}
	
	public Button getAddButton() {
		return (Button) contents.get("AddButton");
	}
	
	public List getClassList() {
		return (List) contents.get("ClassList");
	}
	
	public Button getRemoveButton() {
		return (Button) contents.get("RemoveButton");
	}
}
