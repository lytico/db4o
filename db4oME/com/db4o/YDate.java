/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.reflect.*;


final class YDate extends YLong
{
	
	private static final Date PROTO = new Date(0);
    
    public YDate(YapStream stream) {
        super(stream);
    }
    
    public Object coerce(ReflectClass claxx, Object obj) {
        return canHold(claxx) ? obj : No4.INSTANCE;
    }

	public void copyValue(Object a_from, Object a_to){
		try{
			((Date)a_to).setTime(((Date)a_from).getTime());
		}catch(Exception e){
		}
	}
	
	public Object defaultValue(){
		return PROTO;
	}
	
	public int getID(){
		return 10;
	}
    
    public boolean indexNullHandling() {
        return true;
    }
	
	protected Class primitiveJavaClass(){
		return null;
	}
	
	Object primitiveNull(){
		return null;
	}
	
	Object read1(YapReader a_bytes){
		long ret = readLong(a_bytes);
		if(ret == Long.MAX_VALUE){
			return null;
		}
		return new Date(ret);
	}
	
	public void write(Object a_object, YapReader a_bytes){
		if (a_object == null){
			writeLong(Long.MAX_VALUE,a_bytes);
		} else {
			writeLong(((Date)a_object).getTime(), a_bytes);
		}
	}
    
    public Object current1(){
        return new Date(i_compareTo);
    }
	
	static String now(){
		return Platform4.format(new Date(), true);
	}
	
	long val(Object obj){
		return ((Date)obj).getTime();
	}
	
	boolean isEqual1(Object obj){
		return obj instanceof Date && val(obj) == i_compareTo;
	}
	
	boolean isGreater1(Object obj){
		return obj instanceof Date && val(obj) > i_compareTo;
	}
	
	boolean isSmaller1(Object obj){
		return obj instanceof Date && val(obj) < i_compareTo;
	}
	
	
}
