/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.inside;

public class DrsDebug {
	
	private static boolean production = false;
	
	public static final boolean runEventListenerEmbedded = !production;
	
	public static final boolean verbose = false;
	
	public static final boolean fastReplicationFeaturesMain = true;

	
	public static long timeout(long designed) {
		return designed;
	}

}
