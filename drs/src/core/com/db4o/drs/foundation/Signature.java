/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

import com.db4o.foundation.*;

public class Signature {
	
	public final byte[] bytes;
	
	public Signature(byte[] bytes){
		this.bytes = bytes;
	}
	
	@Override
	public boolean equals(Object obj) {
		if(this == obj){
			return true;
		}
		if(! (obj instanceof Signature)){
			return false;
		}
		Signature other = (Signature) obj;
		return Arrays4.equals(bytes, other.bytes);
	}
	
	@Override
	public int hashCode() {
		int hc = 0;
		for (int i = 0; i < bytes.length; i++) {
			hc <<= 2;
			hc = hc ^ bytes[i]; 
		}	
		return hc;
	}
	
	public String toString(){
		return toString(bytes);
	}

	public static String toString(byte[] bytes) {
		return bytesToString(bytes);
	}

	private static String bytesToString(byte[] bytes) {
		StringBuffer sb = new StringBuffer();
		for (int i = 0; i < bytes.length; i++) {
			char c = (char) bytes[i];
			sb.append(c);
		}	
		return sb.toString();
	}
	
	public String asString(){
		return bytesToString(bytes);
	}
	
}
