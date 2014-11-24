package com.db4o.util;

public class Bits {

	public static boolean contains(int haystack, int needle) {
		return (haystack & needle) != 0;
	}
	
}
