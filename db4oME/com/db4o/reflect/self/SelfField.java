package com.db4o.reflect.self;

import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;

public class SelfField implements ReflectField {

	private String _name;

	private ReflectClass _type;

	private SelfClass _selfclass;

	private SelfReflectionRegistry _registry;

	public SelfField(String name, ReflectClass type, SelfClass selfclass,
			SelfReflectionRegistry registry) {
		_name = name;
		_type = type;
		_selfclass = selfclass;
		_registry = registry;
	}

	public Object get(Object onObject) {
		if (onObject instanceof SelfReflectable) {
			return ((SelfReflectable) onObject).self_get(_name);
		}
		return null;
	}

	public String getName() {
		return _name;
	}

	public ReflectClass getType() {
		return _type;
	}

	public boolean isPublic() {
		return _registry.infoFor(_selfclass.getJavaClass()).fieldByName(_name)
				.isPublic();
	}

	public boolean isStatic() {
		return _registry.infoFor(_selfclass.getJavaClass()).fieldByName(_name)
				.isStatic();
	}

	public boolean isTransient() {
		return _registry.infoFor(_selfclass.getJavaClass()).fieldByName(_name)
				.isTransient();
	}

	public void set(Object onObject, Object value) {
		if (onObject instanceof SelfReflectable) {
			((SelfReflectable) onObject).self_set(_name, value);
		}
	}

	public void setAccessible() {
	}

}
