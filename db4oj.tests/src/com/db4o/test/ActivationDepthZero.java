/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ActivationDepthZero {
	public void configure() {
		Db4o.configure().activationDepth(0);
	}
}
