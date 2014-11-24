/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.dataeditors.db4o;

import java.util.Iterator;
import java.util.LinkedList;

import org.eclipse.swt.widgets.Shell;
import org.eclipse.ve.sweet.CannotSaveException;
import org.eclipse.ve.sweet.fieldviewer.FieldViewerFactory;
import org.eclipse.ve.sweet.fieldviewer.IFieldViewer;
import org.eclipse.ve.sweet.hinthandler.DelegatingHintHandler;
import org.eclipse.ve.sweet.hinthandler.IHintHandler;
import org.eclipse.ve.sweet.objectviewer.*;
import org.eclipse.ve.sweet.reflect.RelaxedDuckType;

import sun.reflect.generics.reflectiveObjects.*;

import com.db4o.ObjectContainer;

/**
 * Db4oObject.
 *
 * @author djo
 */
public class Db4oObject implements IObjectViewer {
    private ObjectContainer database;
    private IObjectViewerFactory editorFactory;

    private Object input = null;
    private IEditedObject inputBean = null;
    
    private Shell shell=null;

    private static final int DEFAULT_REFRESH_DEPTH = 5;
    private int refreshDepth = DEFAULT_REFRESH_DEPTH;

    // The IFieldEditors that are bound to this object
    private LinkedList bindings = new LinkedList();

    private LinkedList objectListeners = new LinkedList();

    private DelegatingHintHandler hintHandler = new DelegatingHintHandler();

    /**
     * @param database
     */
    public Db4oObject(ObjectContainer database,IObjectViewerFactory editorFactory) {
        this.database = database;
        this.editorFactory=editorFactory;
    }
    
    /* (non-Javadoc)
     * @see com.db4o.binding.dataeditors.IObjectEditor#setInput(java.lang.Object)
     */
    public boolean setInput(Object input) {
        if (this.input != null && (validateAndSaveEditedFields()!=null || validateAndSaveObject()!=null || input == null)) {
            return false;
        }
        try {
            commit();
        } catch (CannotSaveException e) {
            throw new RuntimeException("Should be able to save if fields and object verify", e);
        }
        
        if (!fireInputChangingEvent(this.input, input))
            return false;
        
        this.input = input;
        inputBean = (IEditedObject) RelaxedDuckType.implement(IEditedObject.class, input);
        refreshFieldsFromInput();
        
        fireInputChangedEvent(input);
        
        return true;
    }

    private void refreshFieldsFromInput() {
        for (Iterator bindingIter = bindings.iterator(); bindingIter.hasNext();) {
            IFieldViewer controller = (IFieldViewer) bindingIter.next();
            try {
                controller.setInput(getProperty(controller.getPropertyName()));
            } catch (CannotSaveException e) {
                throw new RuntimeException("Should be able to save if fields and object verify", e);
            } catch (NoSuchMethodException e) {
                throw new RuntimeException("Should be able to save if fields and object verify", e);
            }
        }
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#getInput()
     */
    public Object getInput() {
        return input;
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#getProperty(java.lang.String)
     */
    public IPropertyEditor getProperty(String name) throws NoSuchMethodException {
        return Db4oBeanProperty.construct(getInput(), name, database);
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#bind(java.lang.Object, java.lang.String)
     */
    public IFieldViewer bind(Object control, String propertyName) {
        IPropertyEditor propertyEditor;
        try {
            propertyEditor = getProperty(propertyName);
        } catch (NoSuchMethodException e) {
            return null;
        }
        
        IFieldViewer result = FieldViewerFactory.construct(control, this, propertyEditor);
        
        if (result != null) {
            bindings.addLast(result);
        }
        return result;
    }


	public IObjectViewer bind(String propertyName) {
		return bind(propertyName,editorFactory);
	}

	// FIXME
	public IObjectViewer bind(String propertyName, IObjectViewerFactory factory) {
		throw new NotImplementedException();
	}

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#verifyAndSaveEditedFields()
     */
    public String validateAndSaveEditedFields() {
        for (Iterator bindingsIter = bindings.iterator(); bindingsIter.hasNext();) {
            IFieldViewer field = (IFieldViewer) bindingsIter.next();
            if (field.isDirty()) {
                String validateMsg = field.validate();
				if (validateMsg != null) {
                    return validateMsg;
                }
                try {
                    field.save();
                } catch (CannotSaveException e) {
                    return e.getMessage();
                }
            }
        }
        return null;
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#verifyAndSaveObject()
     */
    public String validateAndSaveObject() {
        /*
         * The return type for RelaxedDuckType is false for boolean types if
         * the method does not exist.  So we have to test explicitly here...
         */
        String validateMsg = inputBean.validateObject();
		if (RelaxedDuckType.includes(input, "verifyObject", new Class[] {}) && validateMsg!=null) {
            return validateMsg;
        }
        database.set(input);
        return null;
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.objectviewer.IObjectViewer#setHintHandler(org.eclipse.ve.sweet.hinthandler.IHintHandler)
     */
    public void setHintHandler(IHintHandler hintHandler) {
        this.hintHandler.delegate = hintHandler;
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#commit()
     */
    public void commit() throws CannotSaveException {
        if (input == null) {
            return;
        }
        String validateFieldsMsg = validateAndSaveEditedFields();
		if (validateFieldsMsg!=null)
            throw new CannotSaveException("Unable to save edited fields: "+validateFieldsMsg);
        
        // Ask the bean to verify itself for consistency
        String validateObjectMsg = validateAndSaveObject();
		if (validateObjectMsg!=null)
            throw new CannotSaveException("Unable to save object: "+validateObjectMsg);
        
        // Let the bean itself know it is about to be saved
        inputBean.commit();
        
        // Actually commit the db4o transaction
        database.commit();
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#refresh()
     */
    public void refresh() {
        database.ext().refresh(input, refreshDepth);
        inputBean.refresh();
        refreshFieldsFromInput();
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#rollback()
     */
    public void rollback() {
        database.rollback();
        inputBean.rollback();
        refresh();
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#delete()
     */
    public void delete() {
        inputBean.delete();
        database.delete(input);
    }

    /**
     * @return Returns the refreshDepth.
     */
    public int getRefreshDepth() {
        return refreshDepth;
    }
    

    /**
     * @param refreshDepth The refreshDepth to set.
     */
    public void setRefreshDepth(int refreshDepth) {
        this.refreshDepth = refreshDepth;
    }

    
    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#addObjectListener(org.eclipse.ve.sweet.dataeditors.IObjectListener)
     */
    public void addObjectListener(IEditStateListener listener) {
        objectListeners.add(listener);
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#removeObjectListener(org.eclipse.ve.sweet.dataeditors.IObjectListener)
     */
    public void removeObjectListener(IEditStateListener listener) {
        objectListeners.remove(listener);
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#fireObjectListenerEvent()
     */
    public void fireObjectListenerEvent() {
        for (Iterator listeners = objectListeners.iterator(); listeners.hasNext();) {
            IEditStateListener listener = (IEditStateListener) listeners.next();
            listener.stateChanged(this);
        }
    }
    
    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#isDirty()
     */
    public boolean isDirty() {
        for (Iterator bindingsIter = bindings.iterator(); bindingsIter.hasNext();) {
            IFieldViewer fieldController = (IFieldViewer) bindingsIter.next();
            if (fieldController.isDirty()) {
                return true;
            }
        }
        return false;
    }
    
    private LinkedList inputChangeListeners = new LinkedList();

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#addInputChangeListener(org.eclipse.ve.sweet.dataeditors.IInputChangeListener)
     */
    public void addInputChangeListener(IInputChangeListener listener) {
        inputChangeListeners.add(listener);
    }

    /* (non-Javadoc)
     * @see org.eclipse.ve.sweet.dataeditors.IObjectEditor#removeInputChangeListener(org.eclipse.ve.sweet.dataeditors.IInputChangeListener)
     */
    public void removeInputChangeListener(IInputChangeListener listener) {
        inputChangeListeners.remove(listener);
    }
    
    private boolean fireInputChangingEvent(Object oldInput, Object newInput) {
        for (Iterator i = inputChangeListeners.iterator(); i.hasNext();) {
            IInputChangeListener listener = (IInputChangeListener) i.next();
            if (!listener.inputChanging(oldInput, newInput))
                return false;
        }
        return true;
    }
    
    private void fireInputChangedEvent(Object newInput) {
        for (Iterator i = inputChangeListeners.iterator(); i.hasNext();) {
            IInputChangeListener listener = (IInputChangeListener) i.next();
            listener.inputChanged(newInput);
        }
    }
}
