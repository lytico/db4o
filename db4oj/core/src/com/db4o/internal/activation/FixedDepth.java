/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.activation;

public interface FixedDepth<T extends FixedDepth> {
	T adjustDepthToBorders();
}
