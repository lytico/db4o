package com.db4o.cs.internal.messages;

import com.db4o.internal.*;

public class MInstanceCount extends MsgD implements MessageWithResponse {

	public Msg replyFromServer() {
		synchronized(containerLock()) {
			ClassMetadata clazz = localContainer().classMetadataForID(readInt());
			return Msg.INSTANCE_COUNT.getWriterForInt(transaction(), clazz.indexEntryCount(transaction()));
		}
	}

}
