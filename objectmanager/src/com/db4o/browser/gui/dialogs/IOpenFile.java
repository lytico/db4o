/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.dialogs;

import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Text;

public interface IOpenFile {
    Text getFileName();
    Button getBrowseButton();
    Text getPassword();
    Label getHelpArea();
    Button getReadOnly();
}
