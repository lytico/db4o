/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.mapping;

/**
 * @exclude
 */
public class MappedIDPair {
	private int _orig;
	private int _mapped;

	public MappedIDPair(int orig, int mapped) {
		_orig=orig;
		_mapped=mapped;
	}

	public int orig() {
		return _orig;
	}
	
	public int mapped() {
		return _mapped;
	}
	
	public String toString() {
		return _orig+"->"+_mapped;
	}
}
