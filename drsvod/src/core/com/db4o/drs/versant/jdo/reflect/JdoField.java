package com.db4o.drs.versant.jdo.reflect;

import java.lang.reflect.*;

import com.db4o.internal.*;
import com.db4o.reflect.*;
import com.db4o.reflect.jdk.*;

public class JdoField extends JdkField {

	private final JdoClass jdoClass;

	public JdoField(Reflector reflector, JdoClass jdoClass, Field field) {
		super(reflector, field);
		this.jdoClass = jdoClass;
	}

	@Override
	public Object get(Object object) {
		try {
			return jdoClass.mirror().getFieldValue(object, getName());
		} catch (Exception e) { // TODO more specific exception handling
			throw new ReflectException(toString(), e);
		}
	}

	@Override
	public void set(Object object, Object value) {
		try {
			jdoClass.mirror().setFieldValue(object, getName(), value);
		} catch (Exception e) { // TODO more specific exception handling
			throw new ReflectException(toString(), e);
		}
	}

	public String toString() {
		return "JdoField " + getFieldType().getName() + ":" + getName();
	}

}
