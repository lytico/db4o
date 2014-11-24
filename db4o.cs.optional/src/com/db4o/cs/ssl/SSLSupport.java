/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.cs.ssl;

import javax.net.ssl.*;

import com.db4o.config.*;
import com.db4o.cs.internal.config.*;
import com.db4o.internal.*;

import decaf.*;

@decaf.Ignore(unlessCompatible=Platform.JMX)
public class SSLSupport implements ConfigurationItem {

	private SSLContext _context;

	public SSLSupport() {
		this(null);
	}

	public SSLSupport(SSLContext context) {
		_context = context;
	}
	
	public void apply(InternalObjectContainer container) {
	}

	public void prepare(Configuration config) {
		Db4oClientServerLegacyConfigurationBridge.asNetworkingConfiguration(config).socketFactory(
				new com.db4o.cs.ssl.SSLSocketFactory(_context));
	}
	
}
