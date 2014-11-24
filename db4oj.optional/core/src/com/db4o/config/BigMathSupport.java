package com.db4o.config;

import java.math.*;

import com.db4o.internal.*;
import com.db4o.internal.handlers.*;
import com.db4o.typehandlers.*;

/**
 * Registers type handlers for {@link java.math.BigInteger} and {@link java.math.BigDecimal}
 * providing better performance and support for querying.
 * 
 * @sharpen.ignore
 */
public class BigMathSupport implements ConfigurationItem {

	public void apply(InternalObjectContainer container) {
	}

	public void prepare(Configuration configuration) {
		configuration.registerTypeHandler(new SingleClassTypeHandlerPredicate(BigInteger.class), new BigIntegerTypeHandler());
		configuration.registerTypeHandler(new SingleClassTypeHandlerPredicate(BigDecimal.class), new BigDecimalTypeHandler());
	}

}
