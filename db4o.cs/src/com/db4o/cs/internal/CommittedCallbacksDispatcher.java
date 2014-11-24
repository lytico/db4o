/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;

public class CommittedCallbacksDispatcher implements Runnable {
	
	private boolean _stopped;
	
	private final BlockingQueue _committedInfosQueue;
	
	private final ObjectServerImpl _server;
	
	public CommittedCallbacksDispatcher(ObjectServerImpl server, BlockingQueue committedInfosQueue) {
		_server = server;
		_committedInfosQueue = committedInfosQueue;
	}
	
	public void run () {
		setThreadName();
		messageLoop();
	}

	private void messageLoop() {
	    while(! _stopped){
			MCommittedInfo committedInfos;
			try {
				committedInfos = (MCommittedInfo) _committedInfosQueue.next();
			} catch (BlockingQueueStoppedException e) {
				break;
			}
			_server.broadcastMsg(committedInfos, new BroadcastFilter() {
				public boolean accept(ServerMessageDispatcher dispatcher) {
					return dispatcher.caresAboutCommitted();
				}
			});
		}
    }

	private void setThreadName() {
	    Thread.currentThread().setName("committed callback thread");
    }
	
	public void stop(){
		_committedInfosQueue.stop();
		_stopped = true;
	}

}
