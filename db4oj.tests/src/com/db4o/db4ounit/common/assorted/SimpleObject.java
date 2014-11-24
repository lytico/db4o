/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

public class SimpleObject {

	public String _s;

	public int _i;

	public SimpleObject(String s, int i) {
		_s = s;
		_i = i;
	}

	public boolean equals(Object obj) {
		if (!(obj instanceof SimpleObject)) {
			return false;
		}
		SimpleObject another = (SimpleObject) obj;
		return _s.equals(another._s) && (_i == another._i);

	}

	public int getI() {
		return _i;
	}

	public void setI(int i) {
		_i = i;
	}

	public String getS() {
		return _s;
	}

	public void setS(String s) {
		_s = s;
	}

	public String toString() {
		return _s + ":" + _i;
	}
}