/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.reflect.*;


final class YDouble extends YLong
{
    private static final Double i_primitive = new Double(0);
    
    public YDouble(YapStream stream) {
        super(stream);
    }
    
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toDouble(obj);
    }

	public Object defaultValue(){
		return i_primitive;
	}
	
	public int getID(){
		return 5;
	}
	
	protected Class primitiveJavaClass(){
		return Double.class;
	}
	
	Object primitiveNull(){
		return i_primitive;
	}
	
	Object read1(YapReader a_bytes){
		long ret = readLong(a_bytes);
		if(! Deploy.csharp){
			if(ret == Long.MAX_VALUE){
				return null;
			}
		}
		return new Double(Platform4.longToDouble(ret));
	}
	
	public void write(Object a_object, YapReader a_bytes){
		if (! Deploy.csharp && a_object == null){
			writeLong(Long.MAX_VALUE,a_bytes);
		} else {
			writeLong(Platform4.doubleToLong(((Double)a_object).doubleValue()), a_bytes);
		}
	}
	
	
	// Comparison_______________________
	
	private double i_compareToDouble;
	
	private double dval(Object obj){
		return ((Double)obj).doubleValue();
	}
	
	void prepareComparison1(Object obj){
		i_compareToDouble = dval(obj);
	}
    
    public Object current1(){
        return new Double(i_compareTo);
    }
	
	boolean isEqual1(Object obj){
		return obj instanceof Double && dval(obj) == i_compareToDouble;
	}
	
	boolean isGreater1(Object obj){
		return obj instanceof Double && dval(obj) > i_compareToDouble;
	}
	
	boolean isSmaller1(Object obj){
		return obj instanceof Double && dval(obj) < i_compareToDouble;
	}
	
	
}
