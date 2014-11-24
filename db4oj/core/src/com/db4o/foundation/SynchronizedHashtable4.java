/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class SynchronizedHashtable4 implements DeepClone {
    
    private final Hashtable4 _delegate;
    
    private SynchronizedHashtable4(Hashtable4 delegate_){
        _delegate = delegate_;
    }

    public SynchronizedHashtable4(int size) {
        this(new Hashtable4(size));
    }

    public synchronized Object deepClone(Object obj) {
        return new SynchronizedHashtable4((Hashtable4)_delegate.deepClone(obj));
    }

    public synchronized void put(Object key, Object value) {
        _delegate.put(key, value);
    }

    public synchronized Object get(Object key) {
        return _delegate.get(key);
    }

}
