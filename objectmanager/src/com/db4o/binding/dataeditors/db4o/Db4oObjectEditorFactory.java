/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.dataeditors.db4o;

import org.eclipse.ve.sweet.objectviewer.IObjectViewer;
import org.eclipse.ve.sweet.objectviewer.IObjectViewerFactory;

import com.db4o.ObjectContainer;

public class Db4oObjectEditorFactory implements IObjectViewerFactory {

    private ObjectContainer database;

    public Db4oObjectEditorFactory(ObjectContainer database) {
        this.database = database;
    }
    
    public IObjectViewer construct() {
        return new Db4oObject(database,this);
    }

    public void setDatabase(ObjectContainer database) {
        this.database = database;
    }
}
