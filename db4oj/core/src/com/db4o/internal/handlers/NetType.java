/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;


interface NetType {
	
	public Object defaultValue();
	
	public int typeID();
	
	public void write(Object obj, byte[] bytes, int offset);
	
	public Object read(byte[] bytes, int offset);
	
}
