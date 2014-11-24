/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.dialogs;

import org.eclipse.swt.widgets.*;

public interface ISelectServerPane {
    public Text getHostName();
    public Text getHostPort();
    public Text getUsername();
    public Text getPassword();
    public Button getReadOnly();
}
