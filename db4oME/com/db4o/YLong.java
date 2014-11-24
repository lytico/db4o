/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.reflect.ReflectClass;


class YLong extends YapJavaClass
{

    private static final Long i_primitive = new Long(0);

    public YLong(YapStream stream) {
        super(stream);
    }
    
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toLong(obj);
    }
    
    public Object defaultValue(){
		return i_primitive;
	}
	
	public int getID(){
		return 2;
	}
	
	protected Class primitiveJavaClass(){
		return Long.class;
	}
	
	public int linkLength(){
		return YapConst.YAPLONG_LENGTH;
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
		return new Long(ret);
	}
	
	static final long readLong(YapReader a_bytes){
		long l_return = 0;
		if (Deploy.debug){
			a_bytes.readBegin(YapConst.YAPLONG);
			if(Deploy.debugLong){
				l_return = Long.parseLong(new YapStringIO().read(a_bytes, YapConst.LONG_BYTES).trim()); 
			}else{
				for (int i = 0; i < YapConst.LONG_BYTES; i++){
					l_return = (l_return << 8) + (a_bytes._buffer[a_bytes._offset++] & 0xff);
				}
			}
			a_bytes.readEnd();
		}else{
			for (int i = 0; i < YapConst.LONG_BYTES; i++){
				l_return = (l_return << 8) + (a_bytes._buffer[a_bytes._offset++] & 0xff);
			}
		}
		return l_return;
	}

	public void write(Object a_object, YapReader a_bytes){
		if (! Deploy.csharp && a_object == null){
			writeLong(Long.MAX_VALUE,a_bytes);
		} else {
			writeLong(((Long)a_object).longValue(), a_bytes);
		}
	}
	
	static final void writeLong(long a_long, YapReader a_bytes){
		if(Deploy.debug){
			a_bytes.writeBegin(YapConst.YAPLONG);
			if(Deploy.debugLong){
				String l_s = "                                " + a_long;
				new YapStringIO().write(a_bytes, l_s.substring(l_s.length() - YapConst.LONG_BYTES));
			}
			else{
				for (int i = 0; i < YapConst.LONG_BYTES; i++){
					a_bytes._buffer[a_bytes._offset++] = (byte) (a_long >> ((YapConst.LONG_BYTES - 1 - i) * 8));
				}
			}
			a_bytes.writeEnd();
		}else{
			for (int i = 0; i < YapConst.LONG_BYTES; i++){
				a_bytes._buffer[a_bytes._offset++] = (byte) (a_long >> ((YapConst.LONG_BYTES - 1 - i) * 8));
			}
		}
	}	
	
	static final void writeLong(long a_long, byte[] bytes){
		for (int i = 0; i < YapConst.LONG_BYTES; i++){
			bytes[i] = (byte) (a_long >> ((YapConst.LONG_BYTES - 1 - i) * 8));
		}
	}	
	
	
		
	// Comparison_______________________
	
	protected long i_compareTo;
	
	long val(Object obj){
		return ((Long)obj).longValue();
	}
	
	void prepareComparison1(Object obj){
		i_compareTo = val(obj);
	}
    
    public Object current1(){
        return new Long(i_compareTo);
    }
	
	boolean isEqual1(Object obj){
		return obj instanceof Long && val(obj) == i_compareTo;
	}
	
	boolean isGreater1(Object obj){
		return obj instanceof Long && val(obj) > i_compareTo;
	}
	
	boolean isSmaller1(Object obj){
		return obj instanceof Long && val(obj) < i_compareTo;
	}
	
}
