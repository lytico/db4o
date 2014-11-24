/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers.net;

import com.db4o.reflect.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class NetUInt extends NetSimpleTypeHandler{

	public NetUInt(Reflector reflector) {
		super(reflector, 22, 4);
	}
	
	public String toString(byte[] bytes) {
	    long  l = 0;
		for (int i = 0; i < 4; i++){
			l = (l << 8) + (bytes[i] & 0xff);
		}
		return "" + l ; //$NON-NLS-1$
	}
}
