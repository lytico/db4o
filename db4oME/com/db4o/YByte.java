/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.reflect.*;


final class YByte extends YapJavaClass
{

    static final int LENGTH = 1 + YapConst.ADDED_LENGTH;
	
	private static final Byte i_primitive = new Byte((byte)0);
	
    public YByte(YapStream stream) {
        super(stream);
    }
    
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toSByte(obj);
    }

	public int getID(){
		return 6;
	}
	
	public Object defaultValue(){
		return i_primitive;
	}
	
	boolean isNoConstraint(Object obj, boolean isPrimitive){
		return obj.equals(primitiveNull());
	}
	
	public int linkLength(){
		return LENGTH;
	}

	protected Class primitiveJavaClass(){
		return Byte.class;
	}
	
	Object primitiveNull(){
		return i_primitive;
	}
	
	Object read1(YapReader a_bytes){
		if (Deploy.debug){
			a_bytes.readBegin(YapConst.YAPBYTE);
		}
		byte ret = a_bytes.readByte();
		if (Deploy.debug){
			a_bytes.readEnd();
		}
		return new Byte(ret);
	}
	
	public void write(Object a_object, YapReader a_bytes){
		if(Deploy.debug){
			a_bytes.writeBegin(YapConst.YAPBYTE);
		}
		byte set;
		if (a_object == null){
			set = (byte)0;
		} else {
			set = ((Byte)a_object).byteValue();
		}
		a_bytes.append(set);
		if(Deploy.debug){
			a_bytes.writeEnd();
		}
	}
	
	public boolean readArray(Object array, YapWriter reader) {
        if(array instanceof byte[]){
            reader.readBytes((byte[])array);
            return true;
        }
        
        return false;
	}

    public boolean writeArray(Object array, YapWriter writer) {
        if(array instanceof byte[]){
            writer.append((byte[])array);
            return true;
        }
        return false;
    }   
    

					
	// Comparison_______________________
	
	private byte i_compareTo;
	
	private byte val(Object obj){
		return ((Byte)obj).byteValue();
	}
	
	void prepareComparison1(Object obj){
		i_compareTo = val(obj);
	}
    
    public Object current1(){
        return new Byte(i_compareTo);
    }
	
	boolean isEqual1(Object obj){
		return obj instanceof Byte && val(obj) == i_compareTo;
	}
	
	boolean isGreater1(Object obj){
		return obj instanceof Byte && val(obj) > i_compareTo;
	}
	
	boolean isSmaller1(Object obj){
		return obj instanceof Byte && val(obj) < i_compareTo;
	}
}
