/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.test2;

import java.util.*;

import com.db4o.test.types.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class RTreeSet extends RCollection{
	TEntry entry(){
		return new IntEntry();
	}

	public Object newInstance(){
		return new TreeSet();
	}
}
