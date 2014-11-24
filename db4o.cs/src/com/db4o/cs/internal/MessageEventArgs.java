package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.events.*;

public class MessageEventArgs extends EventArgs {

	private Message _message;

	public MessageEventArgs(Message message) {
	    _message = message;
    }

	/**
	 * @sharpen.property
	 */
	public Message message() {
		return _message;
    }

}
