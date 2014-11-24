/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.messaging;

import com.db4o.*;

/**
 * message recipient for client/server messaging.
 * <br><br>db4o allows using the client/server TCP connection to send
 * messages from the client to the server. Any object that can be
 * stored to a db4o database file may be used as a message.<br><br>
 * See the sample in ../com/db4o/samples/messaging/ on how to
 * use the messaging feature. It is also used to stop the server
 * in ../com/db4o/samples/clientserver/StopServer.java<br><br>
 * <b>See Also:</b><br> 
 * {@link com.db4o.config.Configuration#setMessageRecipient(com.db4o.messaging.MessageRecipient) Configuration.setMessageRecipient(MessageRecipient)}, <br>
 * {@link MessageSender},<br>
 * {@link com.db4o.config.Configuration#getMessageSender()},<br>
 */
public interface MessageRecipient {
	
	/**
	 * the method called upon the arrival of messages.
	 * @param con the ObjectContainer the message was sent to.	 * @param message the message received.	 */
	public void processMessage(ObjectContainer con, Object message);
}
