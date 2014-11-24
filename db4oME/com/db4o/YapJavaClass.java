/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public abstract class YapJavaClass implements TypeHandler4 {
    
    protected final YapStream _stream;
    
    protected ReflectClass _classReflector;
    
    private ReflectClass _primitiveClassReflector;
    
    public YapJavaClass(YapStream stream) {
        _stream = stream;
    }

    private boolean i_compareToIsNull;

    public void appendEmbedded3(YapWriter a_bytes) {
        a_bytes.incrementOffset(linkLength());
    }

    public boolean canHold(ReflectClass claxx) {
        return claxx.equals(classReflector());
    }

    public void cascadeActivation(Transaction a_trans, Object a_object,
        int a_depth, boolean a_activate) {
        // do nothing
    }
    
    public Object coerce(ReflectClass claxx, Object obj) {
        return canHold(claxx) ? obj : No4.INSTANCE;
    }

    public void copyValue(Object a_from, Object a_to) {
        // do nothing
    }

    public abstract Object defaultValue();

    public void deleteEmbedded(YapWriter a_bytes) {
        a_bytes.incrementOffset(linkLength());
    }

    public boolean equals(TypeHandler4 a_dataType) {
        return (this == a_dataType);
    }
    
    public int getType() {
        return YapConst.TYPE_SIMPLE;
    }

    public YapClass getYapClass(YapStream a_stream) {
        return a_stream.i_handlers.i_yapClasses[getID() - 1];
    }

    public Object indexEntry(Object a_object) {
        return a_object;
    }
    
    public boolean indexNullHandling() {
        return false;
    }

    public Object comparableObject(Transaction a_trans, Object a_object) {
        return a_object;
    }

    public void prepareLastIoComparison(Transaction a_trans, Object obj) {
        prepareComparison(obj);
    }

    protected abstract Class primitiveJavaClass();
    
    abstract Object primitiveNull();
    
    public boolean readArray(Object array, YapWriter reader) {
        return false;
    }

    public TypeHandler4 readArrayWrapper(Transaction a_trans, YapReader[] a_bytes) {
        // virtual and do nothing
        return null;
    }

    public Object readQuery(Transaction trans, YapReader reader, boolean toArray)
        throws CorruptionException {
        return read1(reader);
    }

    public Object read(YapWriter writer) throws CorruptionException {
        return read1(writer);
    }

    abstract Object read1(YapReader reader) throws CorruptionException;

    public void readCandidates(YapReader a_bytes, QCandidates a_candidates) {
        // do nothing
    }

    public Object readIndexEntry(YapReader a_reader) {
        try {
            return read1(a_reader);
        } catch (CorruptionException e) {
        }
        return null;
    }
    
    public Object readIndexValueOrID(YapWriter a_writer) throws CorruptionException{
        return read(a_writer);
    }
    
    public ReflectClass classReflector(){
    	
        if(_classReflector != null){
        	return _classReflector;
        }
        _classReflector = _stream.reflector().forClass(defaultValue().getClass());
        Class clazz = primitiveJavaClass();
        if(clazz != null){
        	_primitiveClassReflector = _stream.reflector().forClass(clazz);
        }
    	return _classReflector;  
    }
    
    /** 
     * 
     * classReflector() has to be called first, before this returns a value
     */
    public ReflectClass primitiveClassReflector(){
    	return _primitiveClassReflector;  
    }

    public boolean supportsIndex() {
        return true;
    }

    public abstract void write(Object a_object, YapReader a_bytes);
    
    public boolean writeArray(Object array, YapWriter reader) {
        return false;
    }

    public void writeIndexEntry(YapReader a_writer, Object a_object) {
        write(a_object, a_writer);
    }

    public int writeNew(Object a_object, YapWriter a_bytes) {
        if (Deploy.csharp) {
            if (a_object == null) {
                a_object = primitiveNull();
            }
        }
        write(a_object, a_bytes);
		return -1;
    }

    public YapComparable prepareComparison(Object obj) {
        if (obj == null) {
            i_compareToIsNull = true;
            return Null.INSTANCE;
        }
        i_compareToIsNull = false;
        prepareComparison1(obj);
        return this;
    }
    
    public Object current(){
        if (i_compareToIsNull){
            return null;
        }
        return current1();
    }

    abstract void prepareComparison1(Object obj);
    
    public abstract Object current1();

    public int compareTo(Object obj) {
        if (i_compareToIsNull) {
            if (obj == null) {
                return 0;
            }
            return 1;
        }
        if (obj == null) {
            return -1;
        }
        if (isEqual1(obj)) {
            return 0;
        }
        if (isGreater1(obj)) {
            return 1;
        }
        return -1;
    }

    public boolean isEqual(Object obj) {
        if (i_compareToIsNull) {
            return obj == null;
        }
        return isEqual1(obj);
    }

    abstract boolean isEqual1(Object obj);

    public boolean isGreater(Object obj) {
        if (i_compareToIsNull) {
            return obj != null;
        }
        return isGreater1(obj);
    }

    abstract boolean isGreater1(Object obj);

    public boolean isSmaller(Object obj) {
        if (i_compareToIsNull) {
            return false;
        }
        return isSmaller1(obj);
    }

    abstract boolean isSmaller1(Object obj);

    // redundant, only added to make Sun JDK 1.2's java happy :(
    public abstract int linkLength();
}