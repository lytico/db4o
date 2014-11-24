/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.query.controllers;

import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;

import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.graphics.Point;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Canvas;
import org.eclipse.swt.widgets.Text;
import org.eclipse.ve.sweet.CannotSaveException;
import org.eclipse.ve.sweet.fieldviewer.IFieldViewer;

import com.db4o.binding.browser.FieldConstraintRelationalOperatorFieldController;
import com.db4o.binding.browser.FieldConstraintValueFieldController;
import com.db4o.browser.query.view.IConstraintRow;
import com.db4o.browser.query.view.PrototypeInstanceEditor;
import com.db4o.browser.query.view.QueryBrowserPane;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;

public class QueryBuilderPaneController {

    private com.db4o.objectmanager.model.query.QueryBuilderModel queryModel;
    private QueryBrowserPane queryView;
    
    private int numEditors = 0;
    
    private static final int SPACING=8;

    public QueryBuilderPaneController(com.db4o.objectmanager.model.query.QueryBuilderModel queryModel, QueryBrowserPane queryView) {
        this.queryModel = queryModel;
        this.queryView = queryView;
        
        com.db4o.objectmanager.model.query.QueryPrototypeInstance root = queryModel.getRootInstance();
        buildEditor(root, null);
        layout(queryView);
    }

    private void layout(QueryBrowserPane queryView) {
        final Canvas queryArea = queryView.getQueryArea();
        GridLayout layout = new GridLayout(numEditors, false);
        layout.horizontalSpacing = SPACING;
        layout.verticalSpacing = SPACING;
        layout.marginHeight = SPACING;
        layout.marginWidth = SPACING;
        queryArea.setLayout(layout);
        Point size = queryArea.computeSize(SWT.DEFAULT, SWT.DEFAULT, true);
        queryArea.setBounds(0, 0, size.x, size.y);
        queryArea.layout(true);
    }
    
    private LinkedList controllers = new LinkedList();
    
    private void buildEditor(com.db4o.objectmanager.model.query.QueryPrototypeInstance root, String fieldName) {
        class EditorRow {
            public ReflectField field;
            public IConstraintRow rowEditor;

            public EditorRow(ReflectField field, IConstraintRow rowEditor) {
                this.field = field;
                this.rowEditor = rowEditor;
            }
        }

        if (root == null || root.getType() == null) {
            return;
        }
        
        ++numEditors;
        
        PrototypeInstanceEditor editor = new PrototypeInstanceEditor(queryView.getQueryArea(), SWT.NULL);
        
        // compute a nice title for the editor
        String className = root.getType().getName();
        int lastDotIndex = className.lastIndexOf('.');
        if (lastDotIndex > 0) {
            className = className.substring(lastDotIndex+1);
        }
        editor.setTypeName(fieldName == null ? className : fieldName + " : " + className, root.getType().isInterface());

        // Now expand the fields
        HashMap priorRows = new HashMap();
        ReflectField[] fields = root.getFields();
        
        for (int i = 0; i < fields.length; i++) {
            com.db4o.objectmanager.model.query.FieldConstraint field = root.getConstraint(fields[i]);
            
            /*
             * If we've seen this field name before, it's a refactored field;
             * we must include something to distinguish it from the other version
             */
            String curFieldName = field.field.getName();
            EditorRow priorRow = (EditorRow) priorRows.get(curFieldName);
            if (priorRow != null) {
                curFieldName = "(" + field.field.getFieldType() + ") " + curFieldName;
                String oldFieldName = "(" + priorRow.field.getFieldType().getName() + ") " + priorRow.field.getName();
                priorRow.rowEditor.setFieldName(oldFieldName);
            }
            
            /*
             * Now build the actual field editor row
             */
            final ReflectClass fieldType = field.field.getFieldType();
            IConstraintRow newRow = null;
            
            if (fieldType.isSecondClass()) {
                newRow = editor.addPrimitiveTypeRow(curFieldName, field.field.isPublic());
                // Relational operator...
                IFieldViewer controller;
                controller = new FieldConstraintRelationalOperatorFieldController(newRow.getRelationEditor(), field);
                controllers.add(controller);
                
                // Value...
                controller = new FieldConstraintValueFieldController((Text)newRow.getValueEditor(), field, queryModel.getDatabase());
                controllers.add(controller);
            } else {
                newRow = editor.addObjectReferenceRow(curFieldName, field.field.isPublic());
                newRow.setValue(fieldType.getName() + " >>>");
                Button expandEditor = (Button) newRow.getValueEditor();
                expandEditor.addSelectionListener(new ExpandEditor(field, editor, newRow));
//                buildEditor(field.valueProto());
            }
            
            /*
             * If we didn't find a dupe earlier, put this row in our priorRows
             * HashMap so we can find it if a dupe turns up later.
             */
            if (priorRow == null) {
                priorRows.put(curFieldName, new EditorRow(fields[i], newRow));
            }
        }
    }
    
    private class ExpandEditor implements SelectionListener {

        private com.db4o.objectmanager.model.query.FieldConstraint field;
        private PrototypeInstanceEditor editor;
        private IConstraintRow row;

        public ExpandEditor(com.db4o.objectmanager.model.query.FieldConstraint field, PrototypeInstanceEditor editor, IConstraintRow row) {
            this.field = field;
            this.editor = editor;
            this.row = row;
        }

        public void widgetSelected(SelectionEvent e) {
            field.expand();
            buildEditor(field.valueProto(), field.field.getName());
            row.getValueEditor().setEnabled(false);
            layout(queryView);
        }

        public void widgetDefaultSelected(SelectionEvent e) {
            widgetSelected(e);
        }
        
    }
    
    public void save() throws CannotSaveException {
        for (Iterator controllerIter = controllers.iterator(); controllerIter.hasNext();) {
            IFieldViewer controller = (IFieldViewer) controllerIter.next();
            controller.save();
        }
    }

}
