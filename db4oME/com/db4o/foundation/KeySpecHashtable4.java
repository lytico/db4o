/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class KeySpecHashtable4 extends Hashtable4 {
	private KeySpecHashtable4() {
		super();
	}
	
	public KeySpecHashtable4(int a_size) {
		super(a_size);
	}
	
    public void put(KeySpec spec,byte value) {
    	super.put(spec,new Byte(value));
    }

    public void put(KeySpec spec,boolean value) {
    	super.put(spec,new Boolean(value));
    }

    public void put(KeySpec spec,int value) {
    	super.put(spec,new Integer(value));
    }

    public void put(KeySpec spec, Object value) {
    	super.put(spec,value);
    }

    public byte getAsByte(KeySpec spec) {
    	return ((Byte)get(spec)).byteValue();
    }

    public boolean getAsBoolean(KeySpec spec) {
    	return ((Boolean)get(spec)).booleanValue();
    }

    public int getAsInt(KeySpec spec) {
    	return ((Integer)get(spec)).intValue();
    }

    public String getAsString(KeySpec spec) {
    	return (String)get(spec);
    }

    public Object get(KeySpec spec) {
        Object value=super.get(spec);
        return (value==null ? spec.defaultValue() : value);
    }
    
    public Object deepClone(Object obj) {
    	return deepCloneInternal(new KeySpecHashtable4(), obj);
    }
}
