/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.config;

import com.db4o.*;

/**
 * @exclude
 */
public class Entry implements Compare, Internal4 {
	
	public Object key;
	
	public Object value;
	
	public Object compare(){
		return key;
	}
}
