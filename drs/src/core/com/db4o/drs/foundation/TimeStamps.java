/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

public class TimeStamps {
	
	private long _from;
	
	private long _commit;
	
	public TimeStamps(long from, long commit){
		this._from = from;
		this._commit = commit;
	}
	
	public long to(){
		return _commit - 1;
	}
	
	public long from(){
		return _from;
	}
	
	public long commit(){
		return _commit;
	}

}
