/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr;

public abstract class BinaryExpression implements Expression {

	protected Expression _left;
	protected Expression _right;
	
	public BinaryExpression(Expression left, Expression right) {
		this._left = left;
		this._right = right;
	}
		
	public Expression left() {
		return _left;
	}

	public Expression right() {
		return _right;
	}	
	
	public boolean equals(Object other) {
		if (this == other) {
			return true;
		}
		if (other == null || getClass() != other.getClass()) {
			return false;
		}
		BinaryExpression casted = (BinaryExpression) other;
		return _left.equals(casted._left)&&(_right.equals(casted._right))||_left.equals(casted._right)&&(_right.equals(casted._left));
	}
	
	public int hashCode() {
		return _left.hashCode()+_right.hashCode();
	}
}
