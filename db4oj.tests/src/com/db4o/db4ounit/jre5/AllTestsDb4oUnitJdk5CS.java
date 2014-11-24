/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5;



/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsDb4oUnitJdk5CS {
	public static void main(String[] args) {
		System.exit(new AllTestsDb4oUnitJdk5().runNetworking());
	}
}
