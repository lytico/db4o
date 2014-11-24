/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import com.db4o.internal.*;


/**
 * @exclude
 * @sharpen.ignore
 */
public class SignatureGenerator {
	
	public static String generateSignature() {
		return Platform4.jdk().generateSignature();
	}

}
