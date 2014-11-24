/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;


/**
 * IGraphIteratorSelectionListener. An interface for objects that need to 
 * be notified with an IGraphIterator's selection changes.
 *
 * @author djo
 */
public interface IGraphIteratorSelectionListener {
    /**
     * Query listeners to see if it is okay to change the selection right now
     * 
     * @return true if the selection can be changed; false otherwise.
     */
    boolean canSelectionChange();
    
	/**
	 * Notify listeners that the current selection has changed
	 */
	void selectionChanged();
}