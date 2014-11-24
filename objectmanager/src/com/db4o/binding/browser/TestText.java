/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.browser;

import org.eclipse.swt.SWT;
import org.eclipse.swt.events.VerifyEvent;
import org.eclipse.swt.events.VerifyListener;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Text;

import com.db4o.browser.gui.standalone.IControlFactory;
import com.db4o.browser.gui.standalone.SWTProgram;

public class TestText implements IControlFactory {

    public void createContents(Composite parent) {
        parent.setLayout(new GridLayout());
        final Text text = new Text(parent, SWT.BORDER);
        text.setLayoutData(new GridData(GridData.FILL_HORIZONTAL | GridData.GRAB_HORIZONTAL));
        text.addVerifyListener(new VerifyListener() {
            public void verifyText(VerifyEvent e) {
                System.out.println(text.getText() + " " + e.start + " " + e.end + " " + e.text);
                String currentText = text.getText();
                String newValue = currentText.substring(0, e.start) + e.text + currentText.substring(e.end);
                System.out.println(newValue);
            }
        });    
    }

    public static void main(String[] args) {
        SWTProgram.run(new TestText());
    }
}
