package com.db4o.drs.test.data;

@OptOutRdbms
public class ItemWithUntypedField {
	
	private Object _array;
	
	public ItemWithUntypedField() {
	}
	
	public ItemWithUntypedField(Object array) {
		array(array);
	}

	public void array(Object array) {
		this._array = array;
	}

	public Object array() {
		return _array;
	}
}