/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import java.util.*;

import com.db4o.typehandlers.*;

/**
 * @exclude
 * @sharpen.ignore
 */
public class TypeHandlerConfigurationJDK_1_1 extends TypeHandlerConfiguration {

	public TypeHandlerConfigurationJDK_1_1(Config4Impl config) {
		super(config);
		
        listTypeHandler(new VectorTypeHandler());
        mapTypeHandler(new HashtableTypeHandler());
	}

	public void apply() {
		registerCollection(Vector.class);
		registerMap(Hashtable.class);
	}

}
