/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.config;

import com.db4o.foundation.*;

/**
 * Configures the environment (set of services) used by db4o.
 * 
 * @see Environment
 * @see Environments#my(Class)
 */
public interface EnvironmentConfiguration {

	/**
	 * Contributes a service to the db4o environment.
	 * 
	 * @param service
	 */
	void add(Object service);

}
