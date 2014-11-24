/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.test2;

import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class RHashMap extends RMap{
	public Object newInstance(){
		return new HashMap();
	}
}
