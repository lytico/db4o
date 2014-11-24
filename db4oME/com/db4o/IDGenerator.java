/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

class IDGenerator {
	
	private int id = 0;
	
	int next(){
		id ++;
		if(id > 0){
			return id;
		}
		id = 1;
		return 1;
	}
}

