/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @sharpen.ignore
 */
public class Coercion4 {
	public static Object toByte(Object obj) {
        if(obj instanceof Byte){
            return obj;
        }
        if(obj instanceof Number){
        	Number number=(Number)obj;
        	if(number.byteValue()==number.doubleValue()) {
                return new Byte((number).byteValue());
        	}
        }
        return No4.INSTANCE;
	}

	public static Object toShort(Object obj) {
        if(obj instanceof Short){
            return obj;
        }
        if(obj instanceof Number){
        	Number number=(Number)obj;
        	if(number.shortValue()==number.doubleValue()) {
                return new Short((number).shortValue());
        	}
        }
        return No4.INSTANCE;
	}

	public static Object toInt(Object obj) {
        if(obj instanceof Integer){
            return obj;
        }
        if(obj instanceof Number){
        	Number number=(Number)obj;
        	if(number.intValue()==number.doubleValue()) {
                return new Integer((number).intValue());
        	}
        }
        return No4.INSTANCE;
	}

	public static Object toLong(Object obj) {
        if(obj instanceof Long){
            return obj;
        }
        if(obj instanceof Number){
        	Number number=(Number)obj;
        	if(number.longValue()==number.doubleValue()) {
                return new Long((number).longValue());
        	}
        }
        return No4.INSTANCE;
	}

	public static Object toFloat(Object obj) {
        if(obj instanceof Float){
            return obj;
        }
        if(obj instanceof Number){
        	Number number=(Number)obj;
        	if(number.floatValue()==number.doubleValue()) {
                return new Float((number).floatValue());
        	}
        }
        return No4.INSTANCE;
	}

	public static Object toDouble(Object obj) {
        if(obj instanceof Double){
            return obj;
        }
        if(obj instanceof Number){
        	Number number=(Number)obj;
            return new Double((number).doubleValue());
        }
        return No4.INSTANCE;
	}
}
