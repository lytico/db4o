/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;

/**
 * @exclude
 */
public class ClientAsynchronousMessageProcessor implements Runnable {
	
	private final BlockingQueue _messageQueue;
	
	private boolean _stopped;
	
	public ClientAsynchronousMessageProcessor(BlockingQueue messageQueue){
		_messageQueue = messageQueue;
	}
	
	public void run() {
		while(! _stopped){
			try {
				ClientSideMessage message = (ClientSideMessage) _messageQueue.next();
				if(message != null){
					message.processAtClient();
				}
			} catch (BlockingQueueStoppedException e) {
				break;
			}
		}
	}
	
	public void stopProcessing(){
		_stopped = true;
	}
}
