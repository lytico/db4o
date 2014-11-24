package com.db4o.util;

public class ArrayUtil {

	public static byte[] copy(byte[] data, int from, int to) {
		int length = to - from;
		if(from < 0 || to >= data.length || length < 0) {
			throw new IllegalArgumentException("[" + from + "," + to + "]");
		}
		byte[] copy = new byte[length];
		System.arraycopy(data, from, copy, 0, length);
		return copy;
	}
	
}
