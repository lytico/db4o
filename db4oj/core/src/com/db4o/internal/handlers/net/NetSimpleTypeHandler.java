/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers.net;

import com.db4o.foundation.*;
import com.db4o.internal.handlers.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public abstract class NetSimpleTypeHandler extends NetTypeHandler implements GenericConverter{
	
	private final Reflector _reflector;
	private final String _name;
	private final int _typeID;
	private final int _byteCount;
	
	public NetSimpleTypeHandler(Reflector reflector, int typeID, int byteCount) {
        super();
        _name = dotNetClassName();
        _typeID = typeID;
        _byteCount = byteCount;
        _reflector = reflector;
    }
	
    public ReflectClass classReflector(){
    	if(_classReflector == null){
    		_classReflector = _reflector.forName(_name);		
    	}
    	return _classReflector;  
    }
	
	public Object defaultValue() {
		return new byte[_byteCount];
	}

	public Object primitiveNull() {
		return defaultValue();
	}

	public String getName() {
		return _name;
	}
	
	public int typeID() {
		return _typeID;
	}
	
	public void write(Object obj, byte[] bytes, int offset) {
		byte[] objBytes = bytesFor(obj);
		System.arraycopy(objBytes, 0, bytes, offset, objBytes.length);
	}

	public Object read(byte[] bytes, int offset) {
		byte[] ret = new byte[_byteCount];
		System.arraycopy(bytes, offset, ret, 0, ret.length);
		GenericObject go = new GenericObject((GenericClass)classReflector());
		go.set(0, ret);
		return go;
	}
	
	GenericObject genericObject(Object obj) {
		if(obj != null) {
			return (GenericObject)obj;	
		}
		GenericObject go = new GenericObject((GenericClass)classReflector()); 
		go.set(0, defaultValue());
		return go;
	}
	
	byte[] genericObjectBytes(Object obj) {
		GenericObject go = genericObject(obj);
		return (byte[])go.get(0);
	}
	
	byte[] bytesFor(Object obj) {
		if(obj instanceof byte[]) {
			return (byte[])obj;
		}
		return genericObjectBytes(obj);
	}
	
	public String toString(GenericObject obj) {
		return toString((byte[])obj.get(0));
	}
	
	public String toString(GenericArray array) {
		return Iterators.toString(array.iterator());
	}
	
    /** @param bytes */
	public String toString(byte[] bytes) {
		return ""; //$NON-NLS-1$
	}
	
	
}
