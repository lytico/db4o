/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.messaging;

/**
 * message sender for client/server messaging.
 * <br><br>db4o allows using the client/server TCP connection to send
 * messages from the client to the server. Any object that can be
 * stored to a db4o database file may be used as a message.<br><br>
 * For an example see Reference documentation: <br>
 * http://developer.db4o.com/Resources/view.aspx/Reference/Client-Server/Messaging<br>
 * http://developer.db4o.com/Resources/view.aspx/Reference/Client-Server/Remote_Code_Execution<br><br>
 * <b>See Also:</b><br>
 * {@link com.db4o.config.ClientServerConfiguration#getMessageSender()},<br>
 * {@link MessageRecipient},<br>
 * {@link com.db4o.config.ClientServerConfiguration#setMessageRecipient(MessageRecipient)}
 */
public interface MessageSender {
	
	/**
	 * sends a message to the server.
	 * @param obj the message parameter, any object may be used.
	 */
    public void send(Object obj);
}
