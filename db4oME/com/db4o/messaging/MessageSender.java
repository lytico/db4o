/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.messaging;

/**
 * message sender for client/server messaging.
 * <br><br>db4o allows using the client/server TCP connection to send
 * messages from the client to the server. Any object that can be
 * stored to a db4o database file may be used as a message.<br><br>
 * See the sample in ../com/db4o/samples/messaging/ on how to
 * use the messaging feature. It is also used to stop the server
 * in ../com/db4o/samples/clientserver/StopServer.java<br><br>
 * <b>See Also:</b><br>
 * {@link com.db4o.config.Configuration#getMessageSender()},<br>
 * {@link MessageRecipient},<br>
 * {@link com.db4o.config.Configuration#setMessageRecipient(MessageRecipient)}
 */
public interface MessageSender {
	
	/**
	 * sends a message to the server.
	 * @param obj the message parameter, any object may be used.
	 */
    public void send(Object obj);
}
