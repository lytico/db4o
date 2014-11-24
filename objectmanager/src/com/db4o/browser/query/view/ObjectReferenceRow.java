/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.query.view;

import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CLabel;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Combo;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Display;

import com.db4o.browser.gui.resources.DisposeMgr;
import com.db4o.browser.gui.views.DbBrowserPane;

public class ObjectReferenceRow implements IConstraintRow {
    
    private CLabel fieldName;
    private Button type;

    public ObjectReferenceRow(PrototypeInstanceEditor editor) {
        fieldName = new CLabel(editor, SWT.NULL);
        
        type = new Button(editor, SWT.NULL);
        GridData gd = new GridData();
        gd.horizontalSpan = 2;
        type.setLayoutData(gd);
    }

    public void setFieldName(String fieldName) {
        this.fieldName.setText(fieldName);
    }

    public void setValue(String value) {
        type.setText(value);
    }

    public String getValue() {
        return type.getText();
    }

    public boolean isValueEditable() {
        return false;
    }

    public Control getValueEditor() {
        return type;
    }

    public Combo getRelationEditor() {
        return null;
    }

    public void setPublic(boolean isPublic) {
        if (isPublic) {
            new DisposeMgr(fieldName, new Image(Display.getCurrent(),
                    DbBrowserPane.class.getResourceAsStream("icons/etool16/public_co.gif")));
        } else {
            new DisposeMgr(fieldName, new Image(Display.getCurrent(),
                    DbBrowserPane.class.getResourceAsStream("icons/etool16/private_co.gif")));
        }
    }

}
