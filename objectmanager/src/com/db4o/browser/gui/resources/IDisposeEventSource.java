/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.resources;

import org.eclipse.swt.events.DisposeListener;

public interface IDisposeEventSource {
    void addDisposeListener(DisposeListener disposeListener);
}
