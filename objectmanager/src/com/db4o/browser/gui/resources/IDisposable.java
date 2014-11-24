/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.resources;

/**
 * IDisposable. An interface for SWT objects that can be dispose()d.
 *
 * @author djo
 */
public interface IDisposable {
    public boolean isDisposed();
    public void dispose();
}
