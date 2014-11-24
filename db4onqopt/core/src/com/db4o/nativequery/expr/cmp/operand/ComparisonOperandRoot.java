/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;


public abstract class ComparisonOperandRoot implements ComparisonOperandAnchor {
	public ComparisonOperandAnchor parent() {
		return null;
	}
	
	public final ComparisonOperandAnchor root() {
		return this;
	}
}
