/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.activation;

public final class DepthUtil {

	public static int adjustDepthToBorders(int depth) {
		int depthBorder = 2;
		// TODO when can min value actually occur?
		if (depth > Integer.MIN_VALUE && depth < depthBorder) {
		    return depthBorder;
		}
		return depth;
	}

	private DepthUtil() {
	}
	
}
