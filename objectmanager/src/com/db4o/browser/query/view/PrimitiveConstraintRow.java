/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.query.view;

import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CLabel;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.widgets.Combo;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Text;

import com.db4o.browser.gui.resources.DisposeMgr;
import com.db4o.browser.gui.views.DbBrowserPane;

public class PrimitiveConstraintRow implements IConstraintRow {

    private CLabel fieldName;
    private Combo relationalOperatorChoices;
    private Text fieldValue;
    
    public PrimitiveConstraintRow(PrototypeInstanceEditor editor) {
        fieldName = new CLabel(editor, SWT.NULL);
        
        relationalOperatorChoices = new Combo(editor, SWT.READ_ONLY);

        fieldValue = new Text(editor, SWT.BORDER);
        GridData gd = new GridData(GridData.FILL_HORIZONTAL|GridData.GRAB_HORIZONTAL);
        gd.minimumWidth = 75;
        fieldValue.setLayoutData(gd);
    }

    public void setFieldName(String fieldName) {
        this.fieldName.setText(fieldName);
    }

    public void setValue(String value) {
        fieldValue.setText(value);
    }

    public String getValue() {
        return fieldValue.getText();
    }

    public boolean isValueEditable() {
        return true;
    }

    public Control getValueEditor() {
        return fieldValue;
    }

    public Combo getRelationEditor() {
        return relationalOperatorChoices;
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
