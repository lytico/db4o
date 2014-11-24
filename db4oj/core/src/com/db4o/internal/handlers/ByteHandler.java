/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;

public final class ByteHandler extends PrimitiveHandler {

    static final int LENGTH = 1 + Const4.ADDED_LENGTH;
	
	private static final Byte DEFAULTVALUE = new Byte((byte)0);
	
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toByte(obj);
    }

	public Object defaultValue(){
		return DEFAULTVALUE;
	}
	
	public int linkLength(){
		return LENGTH;
	}

	public Class primitiveJavaClass(){
		return byte.class;
	}
	
	Object read1(ByteArrayBuffer a_bytes){
		if (Deploy.debug){
			a_bytes.readBegin(Const4.YAPBYTE);
		}
		byte ret = a_bytes.readByte();
		if (Deploy.debug){
			a_bytes.readEnd();
		}
		return new Byte(ret);
	}
	
	public void write(Object a_object, ByteArrayBuffer a_bytes){
		if(Deploy.debug){
			a_bytes.writeBegin(Const4.YAPBYTE);
		}
		a_bytes.writeByte(((Byte)a_object).byteValue());
		if(Deploy.debug){
			a_bytes.writeEnd();
		}
	}
					
    public Object read(ReadContext context) {
        if (Deploy.debug) {
            Debug4.readBegin(context, Const4.YAPBYTE);
        }
        
        byte byteValue = context.readByte();
        
        if (Deploy.debug) {
            Debug4.readEnd(context);
        }
        
        return new Byte(byteValue);
    }

    public void write(WriteContext context, Object obj) {
        if (Deploy.debug) {
            Debug4.writeBegin(context, Const4.YAPBYTE);
        }
        
        context.writeByte(((Byte)obj).byteValue());
        
        if (Deploy.debug) {
            Debug4.writeEnd(context);
        }
    }
    
    public PreparedComparison internalPrepareComparison(Object source) {
    	final byte sourceByte = ((Byte)source).byteValue();
    	return new PreparedComparison() {
			public int compareTo(Object target) {
				if(target == null){
					return 1;
				}
				byte targetByte = ((Byte)target).byteValue();
				return sourceByte == targetByte ? 0 : (sourceByte < targetByte ? - 1 : 1); 
			}
		};
    }

}
