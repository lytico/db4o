/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.*;
import com.db4o.internal.*;


/**
 * 
 */
public class MObjectByUuid extends MsgD implements MessageWithResponse {
	public final Msg replyFromServer() {
		long uuid = readLong();
		byte[] signature = readBytes();
		int id = 0;
		Transaction trans = transaction();
		synchronized (containerLock()) {
			try {
				HardObjectReference hardRef = container().getHardReferenceBySignature(trans, uuid, signature);
			    if(hardRef._reference != null){
			        id = hardRef._reference.getID();
			    }
			} catch (Exception e) {
			    if(Deploy.debug){
			        e.printStackTrace();
			    }
			}
		}
		return Msg.OBJECT_BY_UUID.getWriterForInt(trans, id);
	}
}
