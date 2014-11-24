/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;


public final class StringBufferHandler extends StringBasedValueTypeHandlerBase<StringBuffer> {

	public StringBufferHandler() {
		super(StringBuffer.class);
	}

	@Override
	protected String convertObject(StringBuffer obj) {
		return obj.toString();
	}

	@Override
	protected StringBuffer convertString(String str) {
		return new StringBuffer(str);
	}
}