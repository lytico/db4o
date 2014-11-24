package com.db4o.test;

import com.db4o.reflect.self.*;

public abstract class Animal implements Being, SelfReflectable {
	public String _name;

	public Animal(String _name) {
		this._name = _name;
	}
	
	public String name() {
		return _name;
	}

	public Object self_get(String fieldName) {
		if(fieldName.equals("_name")) {
			return _name;
		}
		return null;
	}

	public void self_set(String fieldName,Object value) {
		if(fieldName.equals("_name")) {
			_name=(String)value;
		}
	}
        
        public String toString() {
            return _name;
        }
}
