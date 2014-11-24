package com.db4o.config;

import java.util.UUID;

import com.db4o.internal.*;
import com.db4o.internal.handlers.*;
import com.db4o.typehandlers.*;

/**
 * Registers type handler for {@link java.util.UUID}
 * providing better performance and support for querying.
 * 
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class UuidSupport implements ConfigurationItem {

	public void apply(InternalObjectContainer container) {
	}

	public void prepare(Configuration configuration) {
		configuration.registerTypeHandler(new SingleClassTypeHandlerPredicate(UUID.class), new UuidTypeHandler());
	}

}
