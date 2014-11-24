/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

public final class MCreateClass extends MsgD implements MessageWithResponse {

	public final Msg replyFromServer() {
		try {
			synchronized (containerLock()) {
			    ReflectClass claxx = systemTransaction().reflector().forName(readString());
	            if (claxx != null) {
					ClassMetadata classMetadata = container().produceClassMetadata(claxx);
					if (classMetadata != null) {
						container().checkStillToSet();
						StatefulBuffer returnBytes = container().readStatefulBufferById(systemTransaction(), classMetadata.getID());
						MsgD createdClass = Msg.OBJECT_TO_CLIENT.getWriter(returnBytes);
						return createdClass;
					}
	            }
			}
		} 
		catch (Db4oException e) {
			// TODO: send the exception to the client
		} 
		return Msg.FAILED;
	}
}
