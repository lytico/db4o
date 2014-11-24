/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import java.util.*;

public class RProperties extends RHashtable{
	public Object newInstance(){
		return new Properties();
	}
}
