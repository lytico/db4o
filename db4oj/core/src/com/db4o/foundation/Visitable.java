/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public interface Visitable <T> {
	
	public void accept(Visitor4<T> visitor);

}
