/* Copyright (C) 2004 - 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.marshall.*;

public class FloatHandler0 extends FloatHandler {

	@Override
	public Object read(ReadContext context) {
		Float value = (Float)super.read(context);
		if (value.isNaN()) {
			return null;
		}
		return value;
	}

}
