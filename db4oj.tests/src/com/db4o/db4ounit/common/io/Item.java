/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.io;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Item {
	public int _id;

	public Item(int id) {
		_id = id;
	}
}
