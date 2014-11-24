/* Copyright (C) 2004 - 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.marshall.*;

public class DoubleHandler0 extends DoubleHandler {

	@Override
	public Object read(ReadContext context) {
		Double value = (Double)super.read(context);
		if (value.isNaN()) {
			return null;
		}
		return value;
	}

}
