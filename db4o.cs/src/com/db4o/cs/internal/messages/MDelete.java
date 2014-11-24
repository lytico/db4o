/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.*;
import com.db4o.internal.*;

public final class MDelete extends MsgD implements ServerSideMessage {
	public final void processAtServer() {
		ByteArrayBuffer bytes = this.getByteLoad();
		synchronized (containerLock()) {
			Object obj = container().tryGetByID(transaction(), bytes.readInt());
            boolean userCall = bytes.readInt() == 1;
			if (obj != null) {
				try {
				    container().delete1(transaction(), obj, userCall);
				} catch (Exception e) {
					if (Debug4.atHome) {
						e.printStackTrace();
					}
				}
			}
		}
	}
}