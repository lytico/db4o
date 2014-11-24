/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.metadata;

public class CommitTimestamp extends VodLoidAwareObject {
	
	private long timestamp;
	
	public CommitTimestamp(){
		
	}
	
	public CommitTimestamp(long timestamp){
		this.timestamp = timestamp;
	}
	
	@Override
	public String toString() {
		return "(" + timestamp+ ")";
	}

	public long value() {
		return timestamp;
	}
	
	public void value(long timestamp){
		this.timestamp = timestamp;
	}

}
