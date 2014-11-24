/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.nativequery.expr;

public final class IgnoredExpression implements Expression {

	public static IgnoredExpression INSTANCE = new IgnoredExpression();
	
	private IgnoredExpression() {
	}
	
	public void accept(ExpressionVisitor visitor) {
	}
}
