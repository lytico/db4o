/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.messaging;

import com.db4o.*;
import com.db4o.internal.*;

/**
 * Additional message-related information.
 */
public interface MessageContext {
	
	/**
	 * The container the message was dispatched to.
	 * @sharpen.property
	 */
	ObjectContainer container();
	
	/**
	 * The sender of the current message.
	 * 
	 * The reference can be used to send a reply to it.
	 * @sharpen.property
	 */
	MessageSender sender();
	
	/**
	 * The transaction the current message has been sent with.
	 * @sharpen.property
	 */
	Transaction transaction();
	
}
