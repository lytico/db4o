/* Copyright (C) 2004 - 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public class DoubleHandler extends LongHandler {
	
    private static final Double DEFAULTVALUE = new Double(0);
    
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toDouble(obj);
    }

	public Object defaultValue(){
		return DEFAULTVALUE;
	}
	
	public Class primitiveJavaClass(){
		return double.class;
	}
	
	public Object read(MarshallerFamily mf, StatefulBuffer buffer,
			boolean redirect) throws CorruptionException {
		return mf._primitive.readDouble(buffer);
	}
	
	Object read1(ByteArrayBuffer buffer){
		return primitiveMarshaller().readDouble(buffer);
	}
	
	public void write(Object a_object, ByteArrayBuffer a_bytes){
		a_bytes.writeLong(Platform4.doubleToLong(((Double)a_object).doubleValue()));
	}
	
	@Override
    public Object read(ReadContext context) {
        Long l = (Long)super.read(context);
        return new Double(Platform4.longToDouble(l.longValue()));
    }

    public void write(WriteContext context, Object obj) {
        context.writeLong(Platform4.doubleToLong(((Double)obj).doubleValue()));
    }
    
    public PreparedComparison internalPrepareComparison(Object source) {
    	final double sourceDouble = ((Double)source).doubleValue();
    	return new PreparedComparison() {
			public int compareTo(Object target) {
				if(target == null){
					return 1;
				}
				double targetDouble = ((Double)target).doubleValue();
				return sourceDouble == targetDouble ? 0 : (sourceDouble < targetDouble ? - 1 : 1); 
			}
		};
    }

    
    
}
