/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */
package com.db4o.internal.marshall;

import com.db4o.foundation.*;

public class RawFieldSpec {
    private final AspectType _type;
	private final String _name;
	private final int _fieldTypeID;
	private final boolean _isPrimitive;
	private final boolean _isArray;
	private final boolean _isNArray;
	private final boolean _isVirtual;
	private int _indexID;

	public RawFieldSpec(AspectType aspectType, final String name, final int fieldTypeID, final byte attribs) {
        _type = aspectType;
        _name = name;
		_fieldTypeID = fieldTypeID;
		BitMap4 bitmap = new BitMap4(attribs);
        _isPrimitive = bitmap.isTrue(0);
        _isArray = bitmap.isTrue(1);
        _isNArray = bitmap.isTrue(2);
        _isVirtual=false;
        _indexID=0;
	}

	public RawFieldSpec(AspectType aspectType, final String name) {
	    _type = aspectType;
		_name = name;
		_fieldTypeID = 0;
        _isPrimitive = false;
        _isArray = false;
        _isNArray = false;
        _isVirtual=true;
        _indexID=0;
	}

	public String name() {
		return _name;
	}
	
	public int fieldTypeID() {
		return _fieldTypeID;
	}
	
	public boolean isPrimitive() {
		return _isPrimitive;
	}

	public boolean isArray() {
		return _isArray;
	}

	public boolean isNArray() {
		return _isNArray;
	}
	
	public boolean isVirtual() {
		return _isVirtual;
	}
	
	public boolean isVirtualField() {
		return isVirtual() && isField();
	}
	
	public boolean isField() {
		return _type.isField();
	}

	public int indexID() {
		return _indexID;
	}
	
	void indexID(int indexID) {
		_indexID=indexID;
	}
	
	public String toString() {
		return "RawFieldSpec(" + name() + ")"; 
	}

    public boolean isFieldMetadata() {
        return _type.isFieldMetadata();
    }

	public boolean isTranslator() {
		return _type.isTranslator();
	}
}
