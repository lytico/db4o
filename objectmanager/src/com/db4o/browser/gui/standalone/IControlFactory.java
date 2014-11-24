/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.standalone;

import org.eclipse.swt.widgets.Composite;

/**
 * IControlFactory.  A formalization of the JFace createPartControl()
 * ideom.
 *
 * @author djo
 */
public interface IControlFactory {
    /**
     * Create the contents of some arbitrary Composite.
     * 
     * @param parent The parent Composite.
     */
    public void createContents(Composite parent);
}
