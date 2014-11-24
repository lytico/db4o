/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.encoding.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;



/**
 * @exclude
 */
public class LongHandler extends PrimitiveHandler {

    private static final Long DEFAULTVALUE = new Long(0);

    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toLong(obj);
    }
    
    public Object defaultValue(){
		return DEFAULTVALUE;
	}
	
    public Class primitiveJavaClass(){
		return long.class;
	}
	
	public int linkLength(){
		return Const4.LONG_LENGTH;
	}
	
	public Object read(MarshallerFamily mf, StatefulBuffer buffer,
			boolean redirect) throws CorruptionException {
		return mf._primitive.readLong(buffer);
	}
	
	Object read1(ByteArrayBuffer a_bytes){
		return new Long(a_bytes.readLong());
	}
	
	public void write(Object obj, ByteArrayBuffer buffer){
	    writeLong(buffer, ((Long)obj).longValue());
	}
	
	public static final void writeLong(WriteBuffer buffer, long val){
		if(Deploy.debug){
		    Debug4.writeBegin(buffer, Const4.YAPLONG);
		}
		if(Deploy.debug && Deploy.debugLong){
			String l_s = "                                " + val;
			new LatinStringIO().write(buffer, l_s.substring(l_s.length() - Const4.LONG_BYTES));
		}else{
			for (int i = 0; i < Const4.LONG_BYTES; i++){
			    buffer.writeByte((byte) (val >> ((Const4.LONG_BYTES - 1 - i) * 8)));
			}
		}
		if(Deploy.debug){
		    Debug4.writeEnd(buffer);
		}
	}
	
	public static final long readLong(ReadBuffer buffer){
        long ret = 0;
        if (Deploy.debug){
            Debug4.readBegin(buffer, Const4.YAPLONG);
        }
        if(Deploy.debug && Deploy.debugLong){
            ret = Long.parseLong(new LatinStringIO().read(buffer, Const4.LONG_BYTES).trim()); 
        }else{
            for (int i = 0; i < Const4.LONG_BYTES; i++){
                ret = (ret << 8) + (buffer.readByte() & 0xff);
            }
        }
        if (Deploy.debug){
            Debug4.readEnd(buffer);
        }
        
        return ret;
	}
	
    public Object read(ReadContext context) {
        return new Long(context.readLong());
    }

    public void write(WriteContext context, Object obj) {
        context.writeLong(((Long) obj).longValue());
    }
    
    public static int compare(long first, long second) {
		if (first == second) return 0;
		return first > second ? 1 : -1;
	}

    public PreparedComparison internalPrepareComparison(Object source) {
    	final long sourceLong = ((Long)source).longValue();
    	return new PreparedComparison() {
			public int compareTo(Object target) {
				if(target == null){
					return 1;
				}
				long targetLong = ((Long)target).longValue();
				return compare(sourceLong, targetLong); 
			}
		};
    }

}
