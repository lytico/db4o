/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.slots;

/**
 * @exclude
 */
public class Slot {
    
    public final int _address;
    
    public final int _length;

    public Slot(int address, int length){
        _address = address;
        _length = length;
    }

}
