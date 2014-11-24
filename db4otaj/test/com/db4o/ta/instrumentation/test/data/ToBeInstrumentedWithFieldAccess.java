package com.db4o.ta.instrumentation.test.data;

public class ToBeInstrumentedWithFieldAccess {

	public int _externallyAccessibleInt;
	
	private int _int;
	
	private int[] _intArray;
	
	private char _char;
	
	private double _double;
	
	private float _float;
	
	private long _long;
	
	private byte _byte;
	
	private volatile byte _volatileByte;
	
	private transient Object _transientField;

	public boolean compareID(ToBeInstrumentedWithFieldAccess other) {
		return _int == other._int;
	}
	
	public void setInt(int value) {
		_int = value;
	}
	
	public void setChar(char value) {
		_char = value;
	}
	
	public void setByte(byte value) {
		_byte = value;
	}
	
	public void setVolatileByte(byte value) {
		_volatileByte = value;
	}
	
	public void setLong(long value) {
		_long = value;
	}
	
	public void setFloat(float value) {
		_float = value;
	}
	
	public void setDouble(double value) {
		_double = value;
	}
	
	public void setIntArray(int[] value) {
		_intArray = value;
	}
	
	public int setDoubledAndGetInt(int value) {
		_int = value*2; // arbitrarily long expressions
		return _int;
	}
	
	public void wontBeInstrumented() {
		_transientField = null;
	}
}
