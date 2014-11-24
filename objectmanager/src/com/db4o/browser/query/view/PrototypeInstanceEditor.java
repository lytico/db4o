/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.query.view;

import java.util.HashMap;

import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CLabel;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.layout.RowLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Label;

import com.db4o.browser.gui.resources.DisposeMgr;
import com.db4o.browser.gui.views.DbBrowserPane;


/*

This Composite implements the following layout:

<composite x:style="BORDER">
    <layout x:class="gridLayout" numColumns="3"/>
    <x:children>
        <label text="Car">
            <layoutData x:class="gridData" grabExcessHorizontalSpace="true" horizontalAlignment="GridData.FILL" horizontalSpan="3"/>
        </label>
        <label x:style="SEPARATOR|HORIZONTAL">
            <layoutData x:class="gridData" grabExcessHorizontalSpace="true" horizontalAlignment="GridData.FILL" horizontalSpan="3"/>
        </label>
        
        <!-- Primitive type field example -->
        <label text="model"/>
        <combo text="="/>
        <text x:style="BORDER"/>
        
        <!-- Object type reference -->
        <label text="pilot"/>
        <combo visible="false"/>
        <label text=">>>>>>>>>"/>
        
        <!-- Collection reference (not implemented yet) -->
        <label text="history"/>
        <combo visible="false"/>
        <button text="Select type..."/>

    </x:children>
</composite>

*/

public class PrototypeInstanceEditor extends Composite {
    
    private CLabel typeImage;
    private Label typeName;

    public PrototypeInstanceEditor(Composite parent, int style) {
        super(parent, style | SWT.BORDER);
        setLayout(new GridLayout(3, false));
        
        Composite header = new Composite(this, SWT.NULL);
        header.setLayoutData(horizontalSpanData(3));
        header.setLayout(new RowLayout(SWT.HORIZONTAL));
        
        typeImage = new CLabel(header, SWT.NULL);
        
        typeName = new Label(header, SWT.CENTER);
//        typeName.setLayoutData(horizontalData());
        
        new Label(this, SWT.SEPARATOR | SWT.HORIZONTAL).setLayoutData(horizontalSpanData(3));
    }
    
    private GridData horizontalData() {
        return new GridData(GridData.FILL_HORIZONTAL | GridData.GRAB_HORIZONTAL);
    }
    
    private GridData horizontalSpanData(int horizontalSpan) {
        GridData gd = horizontalData();
        gd.horizontalSpan = horizontalSpan;
        return gd;
    }
    
    public void setTypeName(String typeName, boolean isInterface) {
        this.typeName.setText(typeName);
        if (isInterface) {
            new DisposeMgr(this.typeImage, new Image(Display.getCurrent(),
                    DbBrowserPane.class.getResourceAsStream("icons/obj16/int_obj.gif")));
        } else {
            new DisposeMgr(this.typeImage, new Image(Display.getCurrent(),
                    DbBrowserPane.class.getResourceAsStream("icons/obj16/class_obj.gif")));
        }
    }
    
    private HashMap rows = new HashMap();
    
    public IConstraintRow addPrimitiveTypeRow(String fieldName, boolean isPublic) {
        IConstraintRow row = new PrimitiveConstraintRow(this);
        row.setFieldName(fieldName);
        row.setPublic(isPublic);
        rows.put(fieldName, row);
        return row;
    }
    
    public IConstraintRow addObjectReferenceRow(String fieldName, boolean isPublic) {
        IConstraintRow row = new ObjectReferenceRow(this);
        row.setFieldName(fieldName);
        row.setPublic(isPublic);
        rows.put(fieldName, row);
        return row;
    }
    
    public IConstraintRow getConstraintRow(String fieldName) {
        return (IConstraintRow) rows.get(fieldName);
    }
}
