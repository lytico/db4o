package com.db4o.reflect.self;

public class FieldInfo {
	private String _name;
	private Class _clazz;
	private boolean _isPublic; 
	private boolean _isStatic; 
	private boolean _isTransient;

	
	
	public FieldInfo(String _name, Class _clazz, boolean isPublic, boolean isStatic, boolean isTransient) {
		this._name = _name;
		this._clazz = _clazz;
		this._isPublic = isPublic;
		this._isStatic = isStatic;
		this._isTransient = isTransient;
	}

	public String name() {
		return _name;
	}
	
	public Class type() {
		return _clazz;
	}

	public boolean isPublic() {
		return _isPublic;
	}

	public boolean isStatic() {
		return _isStatic;
	}

	public boolean isTransient() {
		return _isTransient;
	}
}
