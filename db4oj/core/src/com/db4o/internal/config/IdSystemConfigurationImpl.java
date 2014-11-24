/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.config;

import com.db4o.config.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class IdSystemConfigurationImpl implements IdSystemConfiguration {
	
	private final Config4Impl _config;

	public IdSystemConfigurationImpl(Config4Impl config) {
		_config = config;
	}

	public void usePointerBasedSystem() {
		_config.usePointerBasedIdSystem();
	}
	
	public void useStackedBTreeSystem() {
		_config.useStackedBTreeIdSystem();
	}

	public void useInMemorySystem() {
		_config.useInMemoryIdSystem();
		
	}

	public void useCustomSystem(IdSystemFactory factory) {
		_config.useCustomIdSystem(factory);
	}

	public void useSingleBTreeSystem() {
		_config.useSingleBTreeIdSystem();
		
	}

}
