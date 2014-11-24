/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.messaging.*;

import db4ounit.*;

public class ServerToClientTestCase extends MessagingTestCaseBase {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(ServerToClientTestCase.class).run();
	}
	
	static final class AutoReplyRecipient implements MessageRecipient {
		public void processMessage(MessageContext context, Object message) {
			final MessageSender sender = context.sender();
			sender.send("reply: " + message);
		}		
	};
	
	interface ClientWaitLogic {
		void wait(ObjectContainer client1, ObjectContainer client2); 
	}
	
	
	public void testInterleavedCommits() {
		
		assertReplyBehavior(new ClientWaitLogic() {
			public void wait(ObjectContainer client1, ObjectContainer client2) {
				client2.commit();
				client1.commit();
			}
		});
	}
	
	private void assertReplyBehavior(final ClientWaitLogic clientWaitLogic) {
		final MessageCollector collector1 = new MessageCollector();
		final MessageCollector collector2 = new MessageCollector();
		
		final ObjectServer server = openServerWith(new AutoReplyRecipient());
		try {
			final ObjectContainer client1 = openClient("client1", server);			
			try {
				setMessageRecipient(client1, collector1);				
				final MessageSender sender1 = messageSender(client1);
				
				final ObjectContainer client2 = openClient("client2", server);
				try {
					setMessageRecipient(client2, collector2);
					
					final MessageSender sender2 = messageSender(client2);
					sendEvenOddMessages(sender1, sender2);
					
					clientWaitLogic.wait(client1, client2);
					
					try {
						// Give the message processor thread time to dispatch the message.
						Thread.sleep(100);
					} catch (InterruptedException e) {
						e.printStackTrace();
					}
					
					Assert.areEqual("[reply: 0, reply: 2, reply: 4, reply: 6, reply: 8]", collector1.messages.toString());
					Assert.areEqual("[reply: 1, reply: 3, reply: 5, reply: 7, reply: 9]", collector2.messages.toString());
					
				} finally {
					client2.close();
				}
				
			} finally {
				client1.close();
			}
		} finally {
			server.close();
		}
	}

	private void sendEvenOddMessages(final MessageSender even, final MessageSender odd) {
		for (int i=0; i<10; ++i) {
			final Integer message = new Integer(i);
			if (i % 2 == 0) {
				even.send(message);
			} else {
				odd.send(message);
			}
		}
	}
}
