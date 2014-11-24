package com.db4o.internal.handlers;

import java.math.*;

/**
 * @sharpen.ignore
 * @exclude
 */
public class BigDecimalTypeHandler extends ByteArrayRepresentableTypeHandler<BigDecimal> {

	@Override
	protected BigDecimal fromByteArray(byte[] data) {
	    return new BigDecimal(new String(data));
    }
	
	@Override
	protected byte[] toByteArray(BigDecimal value) {
		return value.toString().getBytes();
	}

	@Override
    protected int compare(BigDecimal x, BigDecimal y) {
		return x.compareTo(y);
    }
}
