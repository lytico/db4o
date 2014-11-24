/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.config;

/**
 * allows special comparison behaviour during query evaluation.
 * <br><br>db4o will use the Object returned by the <code>compare()</code>
 * method for all query comparisons.
 */
public interface Compare {
	
	/**
	 * return the Object to be compared during query evaluation.
	 */
	public Object compare();
	
}

