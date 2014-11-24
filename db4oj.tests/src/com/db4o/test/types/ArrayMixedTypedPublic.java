/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import java.util.*;
public class ArrayMixedTypedPublic extends RTest
{
	public Object[] o1;
	public Object[] o2;
	public Object[] o3;	
	public Object[] o4;
	public Object[] o5;
	public Object[] o6;	

	public void set(int ver){
		if(ver == 1){
			o1 = new Boolean[]{new Boolean(true), new Boolean(false), null };
			o2 = null;
			o3 = new Byte[]{ new Byte(Byte.MAX_VALUE), new Byte(Byte.MIN_VALUE), new Byte((byte)0), null};
			o4 = new Float[] {new Float(Float.MAX_VALUE - 1), new Float(Float.MIN_VALUE), new Float(0), null};
			o5 = new String[] {"db4o rules", "cool", "supergreat"};
			o6 = new Date[] {new GregorianCalendar(2000,0,1).getTime(), new GregorianCalendar(2000,0,1).getTime(), new GregorianCalendar(2001,11,31).getTime(), null};
		}else{
			o1 = new Date[] {new GregorianCalendar(2000,0,1).getTime(), new GregorianCalendar(2000,0,1).getTime(), new GregorianCalendar(2001,11,31).getTime(), null};
			o2 = null;
			o3 = new String[] {};
			o4 = new Boolean[]{new Boolean(false), new Boolean(true), new Boolean(true)};
			o5 = new Double[]{new Double(Double.MIN_VALUE), new Double(0)};
			o6 = new Object[]{"ohje", new Double(Double.MIN_VALUE), new Float(4), null};
		}
	}
}
