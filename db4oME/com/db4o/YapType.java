/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

interface YapType {
	
	public Object defaultValue();
	
	public int typeID();
	
	public void write(Object obj, byte[] bytes, int offset);
	
	public Object read(byte[] bytes, int offset);
	
	public int compare(Object compare, Object with);
	
	public boolean isEqual(Object compare, Object with);

}
