package com.db4o.browser.rcp;

import org.eclipse.swt.widgets.Composite;
import org.eclipse.ui.IMemento;
import org.eclipse.ui.part.ViewPart;

import com.db4o.browser.gui.standalone.StandaloneBrowser;
import com.db4o.objectmanager.model.BrowserCore;

public class View extends ViewPart {
	public static final String ID = "com.db4o.browser.rcp.view";

	/**
	 * This is a callback that will allow us to create the viewer and initialize
	 * it.
	 */
	public void createPartControl(Composite parent) {
		new StandaloneBrowser().createContents(parent);
	}
	
	public void saveState(IMemento memento) {
		BrowserCore.getDefault().closing();
		super.saveState(memento);
	}

	/**
	 * Passing the focus request to the viewer's control.
	 */
	public void setFocus() {
	}
}