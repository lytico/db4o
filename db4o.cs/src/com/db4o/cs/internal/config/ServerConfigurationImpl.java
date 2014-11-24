/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.config;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.config.*;
import com.db4o.internal.*;
import com.db4o.internal.config.*;

public class ServerConfigurationImpl extends NetworkingConfigurationProviderImpl implements ServerConfiguration {

	private List<ServerConfigurationItem> _configItems;
	
	public ServerConfigurationImpl(Config4Impl config) {
		super(config);
	}
	
	public CacheConfiguration cache() {
		return new CacheConfigurationImpl(legacy());
	}

	public FileConfiguration file() {
		return Db4oLegacyConfigurationBridge.asFileConfiguration(legacy());
	}

	public CommonConfiguration common() {
		return Db4oLegacyConfigurationBridge.asCommonConfiguration(legacy());
	}

	public void timeoutServerSocket(int milliseconds) {
		legacy().timeoutServerSocket(milliseconds);
	}

	/**
	 * @sharpen.property
	 */
	public int timeoutServerSocket() {
		return legacy().timeoutServerSocket();
	}

	public void addConfigurationItem(ServerConfigurationItem configItem) {
		if(_configItems != null && _configItems.contains(configItem)) {
			return;
		}
		configItem.prepare(this);
		if(_configItems == null) {
			_configItems = new ArrayList<ServerConfigurationItem>();
		}
		_configItems.add(configItem);
	}

	public void applyConfigurationItems(ObjectServer server) {
		if(_configItems == null) {
			return;
		}
		for (ServerConfigurationItem configItem : _configItems) {
			configItem.apply(server);
		}
	}

	public IdSystemConfiguration idSystem() {
		return new IdSystemConfigurationImpl(legacy());
	}
}
