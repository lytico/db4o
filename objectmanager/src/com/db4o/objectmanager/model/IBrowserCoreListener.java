/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;

public interface IBrowserCoreListener {

    void classpathChanged(BrowserCore browserCore);

	void closeEditors(BrowserCore core);

}
