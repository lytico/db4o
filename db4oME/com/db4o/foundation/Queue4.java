/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.foundation;


/**
 * Using the CollectionElement the other way around:
 * CollectionElement.i_next points to the previous element
 * 
 * @exclude
 */
public class Queue4 {
	private List4 _first;
	private List4 _last;
	
    public final void add(Object obj) {
    	List4 ce = new List4(null, obj); 
    	if(_first == null){
    		_last = ce;
    	}else{
    		_first._next = ce;
    	}
    	_first = ce;
    }
    
	public final Object next() {
		if(_last == null){
			return null;
		}
		Object ret = _last._element;
		_last = _last._next;
		if(_last == null){
			_first = null;
		}
		return ret;
	}
    
    public final boolean hasNext(){
        return _last != null;
    }
    
}
