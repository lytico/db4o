/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.classpath;

import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.List;

public interface IClasspathPreferencePagePanel {
    Composite getControl();
    Button getAddJarZipButton();
    Button getAddDirectoryButton();
    List getClasspathList();
    Button getRemoveButton();
}
