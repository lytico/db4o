package com.db4o.cs.internal.messages;

public class MGenerateTransactionTimestamp extends MsgD implements MessageWithResponse{

	public Msg replyFromServer() {
		long forcedTimestamp = readLong();
		long timestamp = transaction().generateTransactionTimestamp(forcedTimestamp);
		return Msg.GENERATE_TRANSACTION_TIMESTAMP.getWriterForLong(transaction(), timestamp);
	}

}
