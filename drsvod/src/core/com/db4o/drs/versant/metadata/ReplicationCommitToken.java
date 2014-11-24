/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.metadata;


public class ReplicationCommitToken extends VodLoidAwareObject {
	
	private int counter;
	
	public void incrementCounter(){
		counter++;
	}
	
}
