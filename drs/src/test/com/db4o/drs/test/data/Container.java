package com.db4o.drs.test.data;


public class Container {
	
	private Value value;
	
	public Container() {
	}
	
	public Container(Value value) {
		this.setValue(value);
	}

	public void setValue(Value value) {
		this.value = value;
	}

	public Value getValue() {
		return value;
	}
}