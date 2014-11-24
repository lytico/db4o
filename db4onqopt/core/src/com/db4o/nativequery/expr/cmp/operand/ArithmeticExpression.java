/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;

import com.db4o.nativequery.expr.cmp.*;



public class ArithmeticExpression implements ComparisonOperand {
	private ArithmeticOperator _op;
	private ComparisonOperand _left;
	private ComparisonOperand _right;

	public ArithmeticExpression(ComparisonOperand left, ComparisonOperand right,ArithmeticOperator op) {
		this._op=op;
		this._left = left;
		this._right = right;
	}

	public ComparisonOperand left() {
		return _left;
	}

	public ComparisonOperand right() {
		return _right;
	}
	
	public ArithmeticOperator op() {
		return _op;
	}
	
	public String toString() {
		return "("+_left+_op+_right+")";
	}
	
	public boolean equals(Object obj) {
		if(this==obj) {
			return true;
		}
		if(obj==null||obj.getClass()!=getClass()) {
			return false;
		}
		ArithmeticExpression casted=(ArithmeticExpression)obj;
		return _left.equals(casted._left)&&_right.equals(casted._right)&&_op.equals(casted._op);
	}
	
	public int hashCode() {
		int hc=_left.hashCode();
		hc*=29+_right.hashCode();
		hc*=29+_op.hashCode();
		return hc;
	}

	public void accept(ComparisonOperandVisitor visitor) {
		visitor.visit(this);
	}
}
