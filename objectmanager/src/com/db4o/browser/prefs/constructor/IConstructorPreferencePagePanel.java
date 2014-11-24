/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.constructor;

import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.List;
import org.eclipse.swt.widgets.Text;

public interface IConstructorPreferencePagePanel {
    Composite getControl();
	Text getConstructorClassInput();
    Button getAddButton();
    List getConstructorClassList();
    Button getRemoveButton();
}
