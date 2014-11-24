/* Copyright (C) 2004 - 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.marshall.*;

public class LongHandler0 extends LongHandler {

	@Override
	public Object read(ReadContext context) {
		Long value = (Long)super.read(context);
		if (value.longValue() == Long.MAX_VALUE) {
			return null;
		}
		return value;
	}
	
}
