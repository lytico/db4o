/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;


/**
 * Composite key for field indexes, first compares on the actual
 * indexed field _value and then on the _parentID (which is a
 * reference to the containing object). 
 * 
 * @exclude
 */
public class FieldIndexKeyImpl implements FieldIndexKey {
	
	private final Object _value;
    
    private final int _parentID;
    
    public FieldIndexKeyImpl(int parentID, Object value){
        _parentID = parentID;
        _value = value;
    }
    
    public int parentID(){
        return _parentID;
    }
    
    public Object value(){
        return _value;
    }
    
    public String toString() {
    	return "FieldIndexKey(" + _parentID + ", " + safeString(_value) + ")";
    }

	private String safeString(Object value) {
		if (null == value) {
			return "null";
		}
		return value.toString();
	}
}
