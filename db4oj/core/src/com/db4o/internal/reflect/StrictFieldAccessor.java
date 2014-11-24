/* Copyright (C) 2008 Versant Inc.   http://www.db4o.com */

package com.db4o.internal.reflect;

import com.db4o.reflect.*;

/**
 * @since 7.7
 */
public class StrictFieldAccessor implements FieldAccessor {

	public Object get(ReflectField field, Object onObject) {
		return field.get(onObject);
	}

	public void set(ReflectField field, Object onObject, Object value) {
		field.set(onObject, value);
	}
}
