package com.db4o.cs.internal.messages;

public class MUseDefaultTransactionTimestamp extends Msg implements ServerSideMessage{

	public void processAtServer() {
		transaction().useDefaultTransactionTimestamp();
	}

}
