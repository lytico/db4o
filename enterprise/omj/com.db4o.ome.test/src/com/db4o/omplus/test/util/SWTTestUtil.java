/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.test.util;

import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;

import com.db4o.omplus.ui.*;

public class SWTTestUtil {

	private SWTTestUtil() {
	}

	@SuppressWarnings("unchecked")
	public static <T extends Widget>  T findChild(Widget root, String name) {
		//System.err.println("SEARCHING " + root + " (" + root.getData(OMESWTUtil.WIDGET_NAME_KEY) + ") FOR " + name);
		if(name.equals(root.getData(OMESWTUtil.WIDGET_NAME_KEY))) {
			return (T) root;
		}
		if(!(root instanceof Composite)) {
			return null;
		}
		Control[] children = ((Composite)root).getChildren();
		for (Control child : children) {
			Widget found = findChild(child, name);
			if(found != null) {
				return (T) found;
			}
		}
		return null;
	}

	public static TabItem  findTabItem(Widget root, String tabFolderName, String tabName) {
		TabFolder tabFolder = findChild(root, tabFolderName);
		for(TabItem tabItem : tabFolder.getItems()) {
			if(tabName.equals(tabItem.getData(OMESWTUtil.WIDGET_NAME_KEY))) {
				return tabItem;
			}
		}
		return null;
	}

	public static void pressButton(Button button) {
		button.notifyListeners(SWT.Selection, new Event());
	}

	public static void selectButton(Button button, boolean selected) {
		button.setSelection(selected);
		button.notifyListeners(SWT.Selection, new Event());
	}

	public static void selectCombo(Combo combo, int idx) {
		combo.select(idx);
		combo.notifyListeners(SWT.Selection, new Event());
	}

	public static void selectList(List list, String[] selection) {
		list.setSelection(selection);
		list.notifyListeners(SWT.Selection, new Event());
	}

	public static Shell findShell(String id) {
		Shell[] shells = PlatformUI.getWorkbench().getDisplay().getShells();
		for (Shell shell : shells) {
			if(id.equals(shell.getData(OMESWTUtil.WIDGET_NAME_KEY))) {
				return shell;
			}
		}
		return null;
	}
	
}
