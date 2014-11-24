package com.db4o.foundation;

public class Coercion4 {
	public static Object toSByte(Object obj) {
        if(obj instanceof Byte){
            return obj;
        }
        return No4.INSTANCE;
	}

	public static Object toShort(Object obj) {
        if(obj instanceof Short){
            return obj;
        }
        return No4.INSTANCE;
	}

	public static Object toInt(Object obj) {
        if(obj instanceof Integer){
            return obj;
        }
        return No4.INSTANCE;
	}

	public static Object toLong(Object obj) {
        if(obj instanceof Long){
            return obj;
        }
        return No4.INSTANCE;
	}

	public static Object toFloat(Object obj) {
        if(obj instanceof Float){
            return obj;
        }
        return No4.INSTANCE;
	}

	public static Object toDouble(Object obj) {
        if(obj instanceof Double){
            return obj;
        }
        return No4.INSTANCE;
	}
}
