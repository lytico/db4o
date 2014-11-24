/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.handlers;

import java.util.*;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;

/**
 * Shared (java/.net) logic for Date handling.
 */
public abstract class DateHandlerBase extends LongHandler {
	
    public Object coerce(ReflectClass claxx, Object obj) {
        return classReflector().isAssignableFrom(claxx) ? obj : No4.INSTANCE;
    }

	public abstract Object copyValue(Object from, Object to);	
	public abstract Object defaultValue();
	public abstract Object nullRepresentationInUntypedArrays();
	
	public Class primitiveJavaClass() {
		return null;
	}
	
	protected Class javaClass() {
		return defaultValue().getClass();
	}
	
	public Object read(MarshallerFamily mf, StatefulBuffer writer, boolean redirect)
			throws CorruptionException {
		return mf._primitive.readDate(writer);
	}
	
	Object read1(ByteArrayBuffer a_bytes) {
		return primitiveMarshaller().readDate(a_bytes);
	}

	public void write(Object a_object, ByteArrayBuffer a_bytes){
        // TODO: This is a temporary fix to prevent exceptions with
        // Marshaller.LEGACY.  
        if(a_object == null){
            a_object = new Date(0);
        }
		a_bytes.writeLong(((Date)a_object).getTime());
	}
    
	public static String now(){
		return Platform4.format(Platform4.now(), true);
	}
	
	@Override
    public Object read(ReadContext context) {
        long milliseconds = ((Long)super.read(context)).longValue();
        return new Date(milliseconds);
    }

    public void write(WriteContext context, Object obj) {
        long milliseconds = ((Date)obj).getTime();
        super.write(context, new Long(milliseconds));
    }
    
    public PreparedComparison internalPrepareComparison(Object source) {
    	final long sourceDate = ((Date)source).getTime();
    	return new PreparedComparison() {
			public int compareTo(Object target) {
				if(target == null){
					return 1;
				}
				long targetDate = ((Date)target).getTime();
				return sourceDate == targetDate ? 0 : (sourceDate < targetDate ? - 1 : 1); 
			}
		};
    }

    
    
}
