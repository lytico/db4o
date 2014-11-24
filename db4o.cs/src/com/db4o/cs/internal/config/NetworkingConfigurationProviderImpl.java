/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.config;

import com.db4o.cs.config.*;
import com.db4o.internal.*;
import com.db4o.internal.config.*;

public class NetworkingConfigurationProviderImpl implements
		NetworkingConfigurationProvider, LegacyConfigurationProvider {

	private final NetworkingConfigurationImpl _networking;

	public NetworkingConfigurationProviderImpl(Config4Impl config) {
		_networking = new NetworkingConfigurationImpl(config);
	}

	public NetworkingConfiguration networking() {
		return _networking;
	}
	
	public Config4Impl legacy() {
		return _networking.config();
	}

}
