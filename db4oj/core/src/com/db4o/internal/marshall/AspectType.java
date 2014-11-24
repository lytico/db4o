/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;


/**
 * @exclude
 */
public class AspectType {
    
    public final byte _id;
    
    public static final AspectType FIELD = new AspectType((byte)1);
    public static final AspectType TRANSLATOR = new AspectType((byte)2);
    public static final AspectType TYPEHANDLER = new AspectType((byte)3);
    
    
    private AspectType(byte id) {
        _id = id;
    }
    
    public static AspectType forByte(byte b){
        switch (b){
            case 1:
                return FIELD;
            case 2:
                return TRANSLATOR;
            case 3:
                return TYPEHANDLER;
            default:
                throw new IllegalArgumentException();
        }
    }
    
    public boolean isFieldMetadata() {
        return isField() || isTranslator();
    }

	public boolean isTranslator() {
		return this == AspectType.TRANSLATOR;
	}

	public boolean isField() {
		return this == AspectType.FIELD;
	}

}
