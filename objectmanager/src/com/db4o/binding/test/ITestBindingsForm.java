/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.test;

import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;

public interface ITestBindingsForm {
    Button getCommit();
    Button getRollback();
    Composite getDataArea();
}
