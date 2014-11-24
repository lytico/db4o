/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.data;

import java.awt.*;

/**
 * @exclude
 */
public class CanNotBeInstrumented extends Point {

	private int _z;
	
	public CanNotBeInstrumented(int x, int y, int z) {
		super(x,y);
		_z = z;
	}
	
}
