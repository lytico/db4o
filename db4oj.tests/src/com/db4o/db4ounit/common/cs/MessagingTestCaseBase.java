/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.config.*;
import com.db4o.foundation.*;
import com.db4o.io.*;
import com.db4o.messaging.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;

public class MessagingTestCaseBase implements TestCase, OptOutMultiSession {
	
	public static final class MessageCollector implements MessageRecipient {
		public final Collection4 messages = new Collection4();
		
		public void processMessage(MessageContext context, Object message) {
			messages.add(message);
		}
	}

	protected MessageSender messageSender(final ObjectContainer client) {
		return client.ext().configure().clientServer().getMessageSender();
	}

	protected ObjectContainer openClient(String clientId, final ObjectServer server) {
		server.grantAccess(clientId, "p");
		
		return com.db4o.cs.Db4oClientServer.openClient(multithreadedClientConfig(), "127.0.0.1", server.ext().port(), clientId, "p");
	}

	private ClientConfiguration multithreadedClientConfig() {
		final ClientConfiguration config = com.db4o.cs.Db4oClientServer.newClientConfiguration();
		config.networking().singleThreadedClient(false);
		return config;
	}

	protected ObjectServer openServerWith(final MessageRecipient recipient) {
		final Configuration config = memoryIoConfiguration();
		setMessageRecipient(config, recipient);
		return openServer(config);
	}

	protected Configuration memoryIoConfiguration() {
		final Configuration config = Db4o.newConfiguration();
		config.storage(new MemoryStorage());
		return config;
	}

	protected ObjectServer openServer(final Configuration config) {
		return com.db4o.cs.Db4oClientServer.openServer(Db4oClientServerLegacyConfigurationBridge.asServerConfiguration(config), "nofile", 0xdb40);
	}

	protected void setMessageRecipient(final ObjectContainer container, final MessageRecipient recipient) {
		setMessageRecipient(container.ext().configure(), recipient);
	}

	private void setMessageRecipient(final Configuration config, final MessageRecipient recipient) {
		config.clientServer().setMessageRecipient(recipient);
	}

}