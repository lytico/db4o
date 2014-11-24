package com.db4o.db4ounit.common.types;

import com.db4o.internal.handlers.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class StringBuilderHandler extends StringBasedValueTypeHandlerBase<StringBuilder> {

	public StringBuilderHandler() {
		super(StringBuilder.class);
	}

	@Override
	protected String convertObject(StringBuilder obj) {
		return obj.toString();
	}

	@Override
	protected StringBuilder convertString(String str) {
		return new StringBuilder(str);
	}
	

}