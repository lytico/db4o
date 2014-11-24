/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.metadata;

public class TimestampToken extends VodLoidAwareObject{
	
	private long value;
	
	public long value(){
		return value;
	}
	
	public void value(long value){
		this.value = value;
	}

}
