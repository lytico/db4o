/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.slots;

/**
 * @exclude
 */
public class Pointer4 {
	
	public final int _id;
	
	public final Slot _slot;
    
    public Pointer4(int id, Slot slot){
    	_id = id;
    	_slot = slot;
    }
    
    public int address(){
        return _slot.address();
    }
    
    public int id(){
        return _id;
    }

    public int length() {
        return _slot.length();
    }
    
}
