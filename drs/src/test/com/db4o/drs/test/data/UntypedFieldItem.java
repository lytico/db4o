package com.db4o.drs.test.data;

public final class UntypedFieldItem {
	
	private Object untyped;
	
	public UntypedFieldItem() {
	}

	public UntypedFieldItem(Object value) {
		setUntyped(value);
	}

	public void setUntyped(Object untyped) {
		this.untyped = untyped;
	}

	public Object getUntyped() {
		return untyped;
	}
}