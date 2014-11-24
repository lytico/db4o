package com.db4o.internal.handlers;

import java.math.*;

/**
 * @sharpen.ignore
 * @exclude
 */
public class BigIntegerTypeHandler extends ByteArrayRepresentableTypeHandler<BigInteger> {

	@Override
	protected BigInteger fromByteArray(byte[] data) {
	    return new BigInteger(data);
    }

	@Override
	protected byte[] toByteArray(BigInteger value) {
	    return value.toByteArray();
    }

	@Override
    protected int compare(BigInteger x, BigInteger y) {
		return x.compareTo(y);
    }
}
