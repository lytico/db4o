/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.metadata;


public class ReplicationCommitRecord extends VodLoidAwareObject {
	
	private DatabaseSignature higherPeer;
	
	private DatabaseSignature lowerPeer;
	
	private long timestamp;
	
	private long[] _concurrentTimestamps;
	
	public ReplicationCommitRecord() {
		
	}
	
	public ReplicationCommitRecord(DatabaseSignature lowerPeer, DatabaseSignature higherPeer){
		this.lowerPeer = lowerPeer;
		this.higherPeer = higherPeer;
	}
	
	public long timestamp(){
		return timestamp;
	}
	
	public void timestamp(long timestamp){
		this.timestamp = timestamp;
	}
	
	public DatabaseSignature higherPeer() {
		return higherPeer;
	}

	public DatabaseSignature lowerPeer() {
		return lowerPeer;
	}

	public void concurrentTimestamps(long[] concurrentTimestamps) {
		_concurrentTimestamps = concurrentTimestamps;
	}

	public long[] concurrentTimestamps() {
		return _concurrentTimestamps;
	}


}
