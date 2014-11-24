/* Copyright (C) 2008 Versant Inc.   http://www.db4o.com */

package com.db4o.internal.reflect;

import com.db4o.reflect.*;

/**
 * @since 7.7
 */
public interface FieldAccessor {
	void set(ReflectField field, Object onObject, Object value);
	Object get(ReflectField field, Object onObject);
}
