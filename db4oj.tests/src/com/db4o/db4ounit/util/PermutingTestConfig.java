/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.util;

public class PermutingTestConfig {

	private Object[][] _values;
	private int[] _indices;
	private boolean _started;
	
	public PermutingTestConfig(Object[][] values) {
		_values=values;
		_indices=new int[_values.length];
		_started=false;
	}
	
	public boolean moveNext() {
		if(!_started) {
			_started=true;
			return true;
		}
		for(int groupIdx=_indices.length-1;groupIdx>=0;groupIdx--) {
			if(_indices[groupIdx]<_values[groupIdx].length-1) {
				_indices[groupIdx]++;
				for(int resetGroupIdx=groupIdx+1;resetGroupIdx<_indices.length;resetGroupIdx++) {
					_indices[resetGroupIdx]=0;
				}
				return true;
			}
		}
		return false;
	}
	
	public Object current(int groupIdx) throws IllegalStateException,IllegalArgumentException {
		if(!_started) {
			throw new IllegalStateException();
		}
		if(groupIdx<0||groupIdx>=_indices.length) {
			throw new IllegalArgumentException();
		}
		return _values[groupIdx][_indices[groupIdx]];
	}
}