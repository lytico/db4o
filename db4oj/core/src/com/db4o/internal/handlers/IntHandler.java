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
public class IntHandler extends PrimitiveHandler {
    

	private static final Integer DEFAULTVALUE = new Integer(0);
    
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toInt(obj);
    }

    public Object defaultValue(){
		return DEFAULTVALUE;
	}
	
    public Class primitiveJavaClass() {
        return int.class;
    }

    public int linkLength() {
        return Const4.INT_LENGTH;
    }

    public Object read(MarshallerFamily mf, StatefulBuffer writer, boolean redirect) throws CorruptionException {
        return mf._primitive.readInteger(writer);
    }

    Object read1(ByteArrayBuffer a_bytes) {
        return new Integer(a_bytes.readInt());
    }    

    public void write(Object obj, ByteArrayBuffer writer) {
        write(((Integer)obj).intValue(), writer);
    }

    public void write(int intValue, ByteArrayBuffer writer) {
        writeInt(intValue, writer);
    }

    public static final void writeInt(int a_int, ByteArrayBuffer a_bytes) {
        if (Deploy.debug) {
            a_bytes.writeBegin(Const4.YAPINTEGER);
            if (Deploy.debugLong) {
                String l_s = "                " + new Integer(a_int).toString();
                new LatinStringIO().write(
                    a_bytes,
                    l_s.substring(l_s.length() - Const4.INTEGER_BYTES));
            } else {
                for (int i = Const4.WRITE_LOOP; i >= 0; i -= 8) {
                    a_bytes._buffer[a_bytes._offset++] = (byte) (a_int >> i);
                }
            }
            a_bytes.writeEnd();
        } else {
            a_bytes.writeInt(a_int);
        }
    }

    public void defragIndexEntry(DefragmentContextImpl context) {
    	context.incrementIntSize();
    }
    
    public Object read(ReadContext context) {
        return new Integer(context.readInt());
    }

    public void write(WriteContext context, Object obj) {
        context.writeInt(((Integer)obj).intValue());
    }

    public PreparedComparison internalPrepareComparison(Object source) {
    	return newPrepareCompare(((Integer)source).intValue());
    }
    
	public PreparedComparison newPrepareCompare(int i) {
		return new PreparedIntComparison(i);
	}
	
    public static int compare(int first, int second) {
		if (first == second) return 0;
		return first > second ? 1 : -1;
	}

	public final class PreparedIntComparison implements PreparedComparison {
    	
		private final int _sourceInt;

		public PreparedIntComparison(int sourceInt) {
			_sourceInt = sourceInt;
		}

		public int compareTo(Object target) {
			if(target == null){
				return 1;
			}
			int targetInt = ((Integer)target).intValue();
			return compare(_sourceInt, targetInt);
		}
	}
	
}