package com.db4o;

public class J2MEUtil {
	public static int lastIndexOf(String haystack,String needle) {
		int foundIdx=-1;
		int idx=-1;
		while((idx=haystack.indexOf(needle,idx+1))>=0) {
			foundIdx=idx;
		}
		return foundIdx;
	}
}
