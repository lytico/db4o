/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr;

import com.db4o.nativequery.expr.cmp.ComparisonOperator;
import com.db4o.nativequery.expr.cmp.operand.*;

public class ComparisonExpression implements Expression {
	private FieldValue _left;
	private ComparisonOperand _right;
	private ComparisonOperator _op;

	public ComparisonExpression(FieldValue left, ComparisonOperand right,ComparisonOperator op) {
		if(left==null||right==null||op==null) {
			throw new NullPointerException();
		}
		this._left = left;
		this._right = right;
		this._op = op;
	}

	public FieldValue left() {
		return _left;
	}
	
	public ComparisonOperand right() {
		return _right;
	}

	public ComparisonOperator op() {
		return _op;
	}

	public String toString() {
		return _left+" "+_op+" "+_right;
	}
	
	public boolean equals(Object other) {
		if (this == other) {
			return true;
		}
		if (other == null || getClass() != other.getClass()) {
			return false;
		}
		ComparisonExpression casted = (ComparisonExpression) other;
		return _left.equals(casted._left)&&_right.equals(casted._right)&&_op.equals(casted._op);
	}

	public int hashCode() {
		return (_left.hashCode()*29+_right.hashCode())*29+_op.hashCode();
	}
	
	public void accept(ExpressionVisitor visitor) {
		visitor.visit(this);
	}
}
