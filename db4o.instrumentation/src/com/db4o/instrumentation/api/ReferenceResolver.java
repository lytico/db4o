/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

import java.lang.reflect.*;

public interface ReferenceResolver {
	Method resolve(MethodRef methodRef) throws InstrumentationException;
}
