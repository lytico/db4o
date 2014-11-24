/* Copyright (C) 2011   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import java.util.*;

import com.db4o.cs.internal.*;
import com.db4o.drs.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;


public class MCommitReplication extends MCommit implements MessageWithResponse {
	
	public Msg replyFromServer() {
		ServerMessageDispatcher dispatcher = serverMessageDispatcher();
		synchronized (containerLock()) {
			LocalTransaction trans = serverTransaction();
			
			long replicationRecordId = readLong();
			long timestamp = readLong();
			
			List concurrentTimestamps = trans.concurrentReplicationTimestamps();
			
			serverMessageDispatcher().server().broadcastReplicationCommit(timestamp, concurrentTimestamps);
			
			ReplicationRecord replicationRecord = (ReplicationRecord) container().getByID(trans, replicationRecordId);
			container().activate(trans, replicationRecord, new FixedActivationDepth(Integer.MAX_VALUE));
			replicationRecord.setVersion(timestamp);
			replicationRecord.concurrentTimestamps(concurrentTimestamps);
			replicationRecord.store(trans);
			container().storeAfterReplication(trans, replicationRecord, container().updateDepthProvider().forDepth(Integer.MAX_VALUE), false);
			 
			trans.commit(dispatcher);
			committedInfo = dispatcher.committedInfo();
			transaction().useDefaultTransactionTimestamp();
		}
		return Msg.OK;
	}

}
