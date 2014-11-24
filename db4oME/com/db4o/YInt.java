/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.reflect.ReflectClass;


/**
 * @exclude
 */
public class YInt extends YapJavaClass {
    
    private static final Integer i_primitive = new Integer(0);
    
    public YInt(YapStream stream) {
        super(stream);
    }
    
    public Object coerce(ReflectClass claxx, Object obj) {
    	return Coercion4.toInt(obj);
    }

    public Object defaultValue(){
		return i_primitive;
	}
	
    public int getID() {
        return 1;
    }

    protected Class primitiveJavaClass() {
        return Integer.class;
    }

    public int linkLength() {
        return YapConst.YAPINT_LENGTH;
    }

    Object primitiveNull() {
        return i_primitive;
    }

    Object read1(YapReader a_bytes) {
        int ret = readInt(a_bytes);
        if (!Deploy.csharp) {
            if (ret == Integer.MAX_VALUE) {
                return null;
            }
        }
        return new Integer(ret);
    }

    static final int readInt(YapReader a_bytes) {
        if (Deploy.debug) {
			int ret = 0;
            a_bytes.readBegin(YapConst.YAPINTEGER);
            if (Deploy.debugLong) {
                ret =
                    Integer.valueOf(new YapStringIO().read(a_bytes, YapConst.INTEGER_BYTES).trim())
                        .intValue();
            } else {
                for (int i = 0; i < YapConst.INTEGER_BYTES; i++) {
                    ret = (ret << 8) + (a_bytes._buffer[a_bytes._offset++] & 0xff);
                }
            }
            a_bytes.readEnd();
			return ret;
        } else {
        	return a_bytes.readInt();
        }
    }

    public void write(Object a_object, YapReader a_bytes) {
        if (!Deploy.csharp && a_object == null) {
            writeInt(Integer.MAX_VALUE, a_bytes);
        } else {
            writeInt(((Integer) a_object).intValue(), a_bytes);
        }
    }

    static final void writeInt(int a_int, YapReader a_bytes) {
        if (Deploy.debug) {
            a_bytes.writeBegin(YapConst.YAPINTEGER);
            if (Deploy.debugLong) {
                String l_s = "                " + new Integer(a_int).toString();
                new YapStringIO().write(
                    a_bytes,
                    l_s.substring(l_s.length() - YapConst.INTEGER_BYTES));
            } else {
                for (int i = YapConst.WRITE_LOOP; i >= 0; i -= 8) {
                    a_bytes._buffer[a_bytes._offset++] = (byte) (a_int >> i);
                }
            }
            a_bytes.writeEnd();
        } else {
            a_bytes.writeInt(a_int);
        }
    }

    // Comparison_______________________

    private int i_compareTo;

    private int val(Object obj) {
        return ((Integer) obj).intValue();
    }

    void prepareComparison1(Object obj) {
        i_compareTo = val(obj);
    }
    
    public Object current1(){
        return new Integer(i_compareTo);
    }

    boolean isEqual1(Object obj) {
        return obj instanceof Integer && val(obj) == i_compareTo;
    }

    boolean isGreater1(Object obj) {
        return obj instanceof Integer && val(obj) > i_compareTo;
    }

    boolean isSmaller1(Object obj) {
        return obj instanceof Integer && val(obj) < i_compareTo;
    }

}