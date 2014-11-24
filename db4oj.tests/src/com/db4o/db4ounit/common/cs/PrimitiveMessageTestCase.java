/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.messaging.*;

import db4ounit.*;

public class PrimitiveMessageTestCase extends MessagingTestCaseBase {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(PrimitiveMessageTestCase.class).run();
	}
	
	public void test() {
		
		final Collection4 expected = new Collection4(new Object[] { "PING", Boolean.TRUE, new Integer(42) });
		final MessageCollector recipient = new MessageCollector();
		final ObjectServer server = openServerWith(recipient);
		try {
			final ObjectContainer client = openClient("client", server);
			try {
				final MessageSender sender = messageSender(client);
				sendAll(expected, sender);
			} finally {
				client.close();
			}
		} finally {
			server.close();
		}
		
		Assert.areEqual(expected.toString(), recipient.messages.toString());
	}

	private void sendAll(final Iterable4 messages, final MessageSender sender) {
		final Iterator4 iterator = messages.iterator();
		while (iterator.moveNext()) {
			sender.send(iterator.current());
		}
	}

}
