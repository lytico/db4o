/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.standalone;

import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.FillLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Label;

public class StatusBar extends Composite {
    
    private Label messageArea;

    public StatusBar(Composite parent, int style) {
        super(parent, style);
        setLayout(new FillLayout());
        messageArea = new Label(this, SWT.NULL);
    }
    
    public void setMessage(String message) {
        messageArea.setText(message);
    }
    
    public void clearMessage() {
        setMessage("");
    }

}
