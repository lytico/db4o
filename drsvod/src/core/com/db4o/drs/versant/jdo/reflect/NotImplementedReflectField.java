package com.db4o.drs.versant.jdo.reflect;

import com.db4o.reflect.*;

public class NotImplementedReflectField implements ReflectField {

	public Object get(Object onObject) {
		throw new java.lang.UnsupportedOperationException();
	}

	public String getName() {
		throw new java.lang.UnsupportedOperationException();
	}

	public ReflectClass getFieldType() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean isPublic() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean isStatic() {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean isTransient() {
		throw new java.lang.UnsupportedOperationException();
	}

	public void set(Object onObject, Object value) {
		throw new java.lang.UnsupportedOperationException();
	}

	public ReflectClass indexType() {
		throw new java.lang.UnsupportedOperationException();
	}

	public Object indexEntry(Object orig) {
		throw new java.lang.UnsupportedOperationException();
	}

}
