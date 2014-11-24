/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;


public class IDGenerator {
	
	private int id = 0;
	
	public int next(){
		id ++;
		if(id > 0){
			return id;
		}
		id = 1;
		return 1;
	}
}

