/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr;


public class BoolConstExpression implements Expression {	
	public static final BoolConstExpression TRUE=new BoolConstExpression(true);
	public static final BoolConstExpression FALSE=new BoolConstExpression(false);

	private boolean _value;
	
	private BoolConstExpression(boolean value) {
		this._value=value;
	}
	
	public boolean value() {
		return _value;
	}
	
	public String toString() {
		return String.valueOf(_value);
	}
	
	public static BoolConstExpression expr(boolean value) {
		return (value ? TRUE : FALSE);
	}

	public void accept(ExpressionVisitor visitor) {
		visitor.visit(this);
	}

	public Expression negate() {
		return (_value ? FALSE : TRUE);
	}
}
