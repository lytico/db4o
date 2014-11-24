/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;


/**
 * get the classname for an internal ID
 */
public final class MClassNameForID extends MsgD implements MessageWithResponse {
    public final Msg replyFromServer() {
        int id = _payLoad.readInt();
        String name = "";
        synchronized (containerLock()) {
			ClassMetadata classMetadata = container().classMetadataForID(id);
			if (classMetadata != null) {
				name = classMetadata.getName();
			}
		}
        return Msg.CLASS_NAME_FOR_ID.getWriterForString(transaction(), name);
    }
}
