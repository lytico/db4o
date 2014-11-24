/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers.net;

import java.math.*;

import com.db4o.reflect.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class NetULong extends NetSimpleTypeHandler{
    
    private static final BigInteger ZERO = new BigInteger("0", 16); //$NON-NLS-1$
    
	private final static BigInteger FACTOR=new BigInteger("100",16); //$NON-NLS-1$
	
	public NetULong(Reflector reflector) {
		super(reflector, 23, 8);
	}
	
	public String toString(byte[] bytes) {
		BigInteger val=ZERO;
		for (int i = 0; i < 8; i++){
			val=val.multiply(FACTOR);
			val=val.add(new BigInteger(String.valueOf(bytes[i] & 0xff),10));
		}
		return val.toString(10);
	}
}
