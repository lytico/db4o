/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.*;

/**
 * extended functionality for the
 * {@link ObjectSet ObjectSet} interface.
 * <br><br>Every db4o {@link ObjectSet ObjectSet}
 * always is an ExtObjectSet so a cast is possible.<br><br>
 * {@link ObjectSet#ext}
 * is a convenient method to perform the cast.<br><br>
 * The ObjectSet functionality is split to two interfaces to allow newcomers to
 * focus on the essential methods.
 */
public interface ExtObjectSet extends ObjectSet {
	
	/**
	 * returns an array of internal IDs that correspond to the contained objects.
	 * <br><br>
	 * @see ExtObjectContainer#getID
	 * @see ExtObjectContainer#getByID
	 */
	public long[] getIDs();
    
    /**
     * returns the item at position [index] in this ObjectSet.
     * <br><br>
     * The object will be activated.
     * @param index the index position in this ObjectSet.  
     * @return the activated object.
     */
    public Object get(int index);
    
    /**
     * skips the specified number of objects without activating them.
     * 
     * Call this method before starting iterating over the iterator returned by {@link ObjectSet#iterator()}.
     * 
     * This method has no effect on calls to {@link #get} 
     * 
     * @since 8.1
     */
    public void skip(int count);
	
}
