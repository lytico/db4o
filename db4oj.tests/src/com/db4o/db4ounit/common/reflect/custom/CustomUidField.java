package com.db4o.db4ounit.common.reflect.custom;

import com.db4o.foundation.*;
import com.db4o.reflect.*;

public class CustomUidField implements ReflectField {

	public CustomClassRepository _repository;

	public CustomUidField() {
	}

	public CustomUidField(CustomClassRepository repository) {
		_repository = repository;
	}

	public Object get(Object onObject) {
		return entry(onObject).uid;
	}

	private PersistentEntry entry(Object onObject) {
		return ((PersistentEntry)onObject);
	}

	public ReflectClass getFieldType() {
		return _repository.forFieldType(java.lang.Object.class);
	}

	public String getName() {
		return "uid";
	}

	public Object indexEntry(Object orig) {
		throw new NotImplementedException();
	}

	public ReflectClass indexType() {
		throw new NotImplementedException();
	}

	public boolean isPublic() {
		return true;
	}

	public boolean isStatic() {
		return false;
	}

	public boolean isTransient() {
		return false;
	}

	public void set(Object onObject, Object value) {
		entry(onObject).uid = value;
	}

	public String toString() {
		return "CustomUidField()";
	}

}
