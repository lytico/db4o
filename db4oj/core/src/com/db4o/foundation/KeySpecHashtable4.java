/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class KeySpecHashtable4 implements DeepClone {
    
    private SynchronizedHashtable4 _delegate;
    
	private KeySpecHashtable4(SynchronizedHashtable4 delegate_) {
		_delegate = delegate_;
	}
	
	public KeySpecHashtable4(int size) {
	    this(new SynchronizedHashtable4(size));
	}
	
    public void put(KeySpec spec,byte value) {
    	_delegate.put(spec,new Byte(value));
    }

    public void put(KeySpec spec,boolean value) {
    	_delegate.put(spec,new Boolean(value));
    }

    public void put(KeySpec spec,int value) {
    	_delegate.put(spec,new Integer(value));
    }

    public void put(KeySpec spec, Object value) {
    	_delegate.put(spec,value);
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

    public long getAsLong(KeySpec spec) {
    	return ((Long)get(spec)).longValue();
    }

    public TernaryBool getAsTernaryBool(KeySpec spec) {
    	return (TernaryBool)get(spec);
    }

    public String getAsString(KeySpec spec) {
    	return (String)get(spec);
    }

    public synchronized Object get(KeySpec spec) {
        Object value=_delegate.get(spec);
        if(value == null){
            value = spec.defaultValue();
            if(value != null){
                _delegate.put(spec, value);
            }
        }
        return value;
    }
    
    public Object deepClone(Object obj) {
    	return new KeySpecHashtable4((SynchronizedHashtable4) _delegate.deepClone(obj));
    }
}
