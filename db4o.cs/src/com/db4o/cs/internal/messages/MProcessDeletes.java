/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.*;
import com.db4o.ext.*;

public class MProcessDeletes extends Msg implements ServerSideMessage {
	
	public final void processAtServer() {
		
		container().withTransaction(transaction(), new Runnable() { public void run() {
			try {
				transaction().processDeletes();
			} catch (Db4oException e) {
				// Don't send the exception to the user because delete is asynchronous
				if(Debug4.atHome){
					e.printStackTrace();
				}
			}
		}});
	}
	
}
