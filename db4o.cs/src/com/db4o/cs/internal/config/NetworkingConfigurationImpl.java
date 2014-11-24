/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.config;

import java.util.*;

import com.db4o.cs.config.*;
import com.db4o.cs.foundation.*;
import com.db4o.internal.*;
import com.db4o.messaging.*;

public class NetworkingConfigurationImpl implements NetworkingConfiguration {

	protected final Config4Impl _config;

	NetworkingConfigurationImpl(Config4Impl config) {
		_config = config;
	}

	public Config4Impl config() {
		return _config;
	}

	public void batchMessages(boolean flag) {
		_config.batchMessages(flag);
	}

	public void maxBatchQueueSize(int maxSize) {
		_config.maxBatchQueueSize(maxSize);
	}

	public void singleThreadedClient(boolean flag) {
		_config.singleThreadedClient(flag);
	}

	public void messageRecipient(MessageRecipient messageRecipient) {
		_config.setMessageRecipient(messageRecipient);
	}

	public void clientServerFactory(ClientServerFactory factory) {
		_config.environmentContributions().add(factory);
	}

	public ClientServerFactory clientServerFactory() {
		final ClientServerFactory configuredFactory = my(ClientServerFactory.class);
		if (null == configuredFactory) {
			return new StandardClientServerFactory();
		}
		return configuredFactory;
	}
	
	public Socket4Factory socketFactory() {
		final Socket4Factory configuredFactory = my(Socket4Factory.class);
		if (null == configuredFactory) {
			return new StandardSocket4Factory();
		}
		return configuredFactory;
	}
	
	public void socketFactory(Socket4Factory factory) {
		_config.environmentContributions().add(factory);
	}

	private <T> T my(Class<T> type) {
		List environmentContributions = _config.environmentContributions();
		for (int i = environmentContributions.size() - 1; i >= 0 ; i--) {
			Object o = environmentContributions.get(i);
			if (type.isInstance(o)) {
				return type.cast(o);
			}
		}
		return null;
	}
}