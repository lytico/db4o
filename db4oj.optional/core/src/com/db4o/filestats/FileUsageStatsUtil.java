/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */

package com.db4o.filestats;

/**
* @exclude
*/
@decaf.Ignore(decaf.Platform.JDK11)
public final class FileUsageStatsUtil {

	private final static String PADDING = "                    ";
	
	private FileUsageStatsUtil() {
	}

	public static String formatLine(String label, long amount) {
		return padLeft(label, 20) + ": " + padLeft(String.valueOf(amount), 12) + "\n";
	}

	private static String padLeft(String val, int len) {
		return PADDING.substring(0, len - val.length()) + val;
	}
}
