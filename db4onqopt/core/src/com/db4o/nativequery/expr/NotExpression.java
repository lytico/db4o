/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr;


public class NotExpression implements Expression {
	private Expression _expr;

	public NotExpression(Expression expr) {
		this._expr = expr;
	}
	
	public String toString() {
		return "!("+_expr+")";
	}

	public Expression expr() {
		return _expr;
	}
	
	public boolean equals(Object other) {
		if (this == other) {
			return true;
		}
		if (other == null || getClass() != other.getClass()) {
			return false;
		}
		NotExpression casted = (NotExpression) other;
		return _expr.equals(casted._expr);
	}
	
	public int hashCode() {
		return -_expr.hashCode();
	}

	public void accept(ExpressionVisitor visitor) {
		visitor.visit(this);
	}
}
