/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.browser;

import org.eclipse.swt.widgets.Combo;
import org.eclipse.ve.sweet.CannotSaveException;
import org.eclipse.ve.sweet.fieldviewer.IFieldViewer;
import org.eclipse.ve.sweet.hinthandler.IHintHandler;
import org.eclipse.ve.sweet.objectviewer.IPropertyEditor;

import com.db4o.objectmanager.model.query.FieldConstraint;

/**
 * FieldConstraintRelationalOperatorFieldController.
 *
 * (cf. Donaudampfschiffskapitaensmuetzenbesitzer...)
 *
 * @author djo
 */
public class FieldConstraintRelationalOperatorFieldController implements IFieldViewer {

    private Combo ui;
    private FieldConstraint constraint;
    private boolean dirty=false;

    public FieldConstraintRelationalOperatorFieldController(Combo ui, FieldConstraint constraint) {
        this.ui = ui;
        this.constraint = constraint;
        
        for (int i = 0; i < com.db4o.objectmanager.model.query.RelationalOperator.OPERATORS.length; i++) {
            ui.add(com.db4o.objectmanager.model.query.RelationalOperator.OPERATORS[i].name());
        }
        ui.select(0);
    }
    

    public String getPropertyName() {
        return "RelationalOperator";
    }

    public boolean isDirty() {
        return dirty;
    }

    public void setDirty(boolean dirty) {
        this.dirty = dirty;
    }

    public void undo() {
        // Not needed for this implementation
    }

    public void save() throws CannotSaveException {
        constraint.relation = com.db4o.objectmanager.model.query.RelationalOperator.OPERATORS[ui.getSelectionIndex()];
    }

    public String validate() {
        return null;
    }


    public void setInput(IPropertyEditor input) throws CannotSaveException {
        // Not needed for this implementation
    }

    public IPropertyEditor getInput() {
        return null;
    }
    
    public void setHintHandler(IHintHandler hintHandler) {
        // Not needed for this implementation
    }

}
