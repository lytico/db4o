/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.config;

import com.db4o.types.*;

/**
 * 
 * @exclude
 */
public class Entry implements Compare, SecondClass
{
	public Object key;
	public Object value;
	
	public Object compare(){
		return key;
	}
}
