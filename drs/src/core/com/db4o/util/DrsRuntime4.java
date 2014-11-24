/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.util;

public class DrsRuntime4 {

	public static boolean runningOnWindows() {
		String osName = System.getProperty("os.name");
		if(osName == null) {
			return false;
		}
		return osName.indexOf("Win") >= 0;
	}
	
}
