/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.config;

import com.db4o.*;

/**
 * A provider for custom database names.
 */
public interface NameProvider {
	/**
	 * Derives a name for the given {@link ObjectContainer}. This method will be called when
	 * database startup has completed, i.e. the method will see a completely initialized {@link ObjectContainer}.
	 * Any code invoked during the startup process (for example {@link ConfigurationItem} instances) will still
	 * see the default naming.
	 */
	String name(ObjectContainer db);
}
