/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.config;

import com.db4o.config.*;
import com.db4o.internal.*;

public class Db4oLegacyConfigurationBridge {

	public static EmbeddedConfiguration asEmbeddedConfiguration(Configuration legacy) {
		return new EmbeddedConfigurationImpl(legacy);
	}

	public static CommonConfiguration asCommonConfiguration(Configuration config) {
		return new CommonConfigurationImpl((Config4Impl) config);
	}

	public static Config4Impl asLegacy(final Object config) {
		return ((LegacyConfigurationProvider)config).legacy();
	}

	public static FileConfiguration asFileConfiguration(Configuration config) {
		return new FileConfigurationImpl((Config4Impl)config);
	}
	
	public static IdSystemConfiguration asIdSystemConfiguration(Configuration config){
		return new IdSystemConfigurationImpl((Config4Impl) config);
	}

}
