/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.query.view;

import java.util.Map;

import org.eclipse.swt.widgets.Canvas;
import org.eclipse.swt.widgets.Composite;

import com.db4o.browser.gui.views.DbBrowserPane;
import com.swtworkbench.community.xswt.XSWT;

public class QueryBrowserPane extends DbBrowserPane {

    public QueryBrowserPane(Composite parent, int style) {
        super(parent, style);
    }
    
    protected Map createContents() {
        return XSWT.createl(this, "query.xswt", getClass());
    }
    
    public Canvas getQueryArea() {
        return (Canvas) contents.get("QueryArea");
    }

}

