/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

import com.db4o.ta.*;

/**
 * Ordering by non primitive fields works only for classes that implement the {@link Activatable} interface 
 * and {@link TransparentActivationSupport} is enabled.  
 */
public class UnsupportedOrderingException extends Db4oRecoverableException {

	public UnsupportedOrderingException(String msg) {
		super(msg);
	}
}
