/* Copyright (C) 2008 Versant Inc.   http://www.db4o.com */
package com.db4o.internal.reflect;

import com.db4o.ext.*;
import com.db4o.reflect.*;

/**
 * @since 7.7
 */
public class LenientFieldAccessor implements FieldAccessor {

	public Object get(ReflectField field, Object onObject) {
		try {
			return field.get(onObject);
		}
		catch(Db4oException e){			
			return null;
		}		
	}

	public void set(ReflectField field, Object onObject, Object value) {
		try {
			field.set(onObject, value);
		}
		catch(Db4oException e) {			
		}
	}
}
