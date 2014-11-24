/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package db4ounit.extensions.concurrency;

import db4ounit.extensions.*;

public class ConcurrencyConventions {

	static String checkMethodNameFor(String testMethodName) {
		int testPrefixLength = testPrefix().length();
		String subMethodName = testMethodName.substring(testPrefixLength);
		return checkPrefix() + subMethodName;
	}

	private static String checkPrefix() {
		if (Db4oUnitPlatform.isPascalCase()) {
			return "Check";
		}
		return "check";
	}

	public static String testPrefix() {
		if (Db4oUnitPlatform.isPascalCase()) {
			return "Conc";
		}
		return "conc";
	}

}
