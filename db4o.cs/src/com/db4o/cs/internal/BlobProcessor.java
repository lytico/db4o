/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;

class BlobProcessor implements Runnable {
	
	private ClientObjectContainer			stream;
	private Queue4 				queue = new NonblockingQueue();
	private boolean				terminated = false;
	
	BlobProcessor(ClientObjectContainer aStream){
		stream = aStream;
	}

	void add(MsgBlob msg){
		synchronized(queue){
			queue.add(msg);
		}
	}
	
	synchronized boolean isTerminated(){
		return terminated;
	}
	
	public void run(){
		try{
			Socket4Adapter socket = stream.createParallelSocket();
			
			MsgBlob msg = null;
			
			// no blobLock synchronisation here, since our first msg is valid
			synchronized(queue){
				msg = (MsgBlob)queue.next();
			}
			
			while(msg != null){
				msg.write(socket);
				msg.processClient(socket);
				synchronized(stream._blobLock){
					synchronized(queue){
						msg = (MsgBlob)queue.next();
					}
					if(msg == null){
						terminated = true;
						Msg.CLOSE_SOCKET.write(socket);
						try{
							socket.close();
						}catch(Exception e){
						}
					}
				}
			}
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
}
