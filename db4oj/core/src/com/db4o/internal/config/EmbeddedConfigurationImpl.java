package com.db4o.internal.config;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;

public class EmbeddedConfigurationImpl implements EmbeddedConfiguration, LegacyConfigurationProvider {

	private final Config4Impl _legacy;
	private List<EmbeddedConfigurationItem> _configItems;

	public EmbeddedConfigurationImpl(Configuration legacy) {
		_legacy = (Config4Impl) legacy;
    }

	public CacheConfiguration cache() {
		return new CacheConfigurationImpl(_legacy);
	}
	
	public FileConfiguration file() {
		return new FileConfigurationImpl(_legacy);
	}

	public CommonConfiguration common() {
		return Db4oLegacyConfigurationBridge.asCommonConfiguration(legacy());
	}

	public Config4Impl legacy() {
		return _legacy;
	}

	public void addConfigurationItem(EmbeddedConfigurationItem configItem) {
		if(_configItems != null && _configItems.contains(configItem)) {
			return;
		}
		configItem.prepare(this);
		if(_configItems == null) {
			_configItems = new ArrayList<EmbeddedConfigurationItem>();
		}
		_configItems.add(configItem);
	}

	public void applyConfigurationItems(EmbeddedObjectContainer container) {
		if(_configItems == null) {
			return;
		}
		for (EmbeddedConfigurationItem configItem : _configItems) {
			configItem.apply(container);
		}
	}

	public IdSystemConfiguration idSystem() {
		return new IdSystemConfigurationImpl(_legacy);
	}

}
