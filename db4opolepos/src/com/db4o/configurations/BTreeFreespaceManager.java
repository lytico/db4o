/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.configurations;

import org.polepos.framework.*;

import com.db4o.internal.*;

public class BTreeFreespaceManager implements ConfigurationSetting {

	public String name() {
		return "BTreeFreespaceManager";
	}
	
	public void apply(Object config) {
		((Config4Impl)config).freespace().useBTreeSystem();
	}

}
