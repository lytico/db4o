/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.resources;

import org.eclipse.swt.graphics.Cursor;

public interface ICursorHolder extends IDisposeEventSource {
    void setCursor(Cursor cursor);
}
