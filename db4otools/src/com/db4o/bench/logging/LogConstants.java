/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.bench.logging;

import java.util.*;


public class LogConstants {

	public final static String READ_ENTRY = "READ ";
	public static final String WRITE_ENTRY = "WRITE ";
	public static final String SYNC_ENTRY = "SYNC ";
	
	public static final String[] ALL_CONSTANTS = {READ_ENTRY, WRITE_ENTRY, SYNC_ENTRY};
	
	public static final String SEPARATOR = ",";
	
	public static Set allEntries() {
		HashSet entries = new HashSet();
		entries.addAll(Arrays.asList(ALL_CONSTANTS));
		return entries;
	}
}
