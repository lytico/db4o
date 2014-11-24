/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.activation;

/**
 * @sharpen.enum
 */
public final class ActivationPurpose {
	
	public static final ActivationPurpose READ = new ActivationPurpose();
	
	public static final ActivationPurpose WRITE = new ActivationPurpose();
	
	private ActivationPurpose() {
	}

}
