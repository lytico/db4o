/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class IntIdGenerator {
	
	private int _current = 1;
	
	public int next(){
		_current ++;
    	if(_current < 0){
    		_current = 1;
    	}
    	return _current;
	}

}
