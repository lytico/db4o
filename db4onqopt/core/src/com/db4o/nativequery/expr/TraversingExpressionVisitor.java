/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr;

import com.db4o.nativequery.expr.cmp.operand.*;

public class TraversingExpressionVisitor implements ExpressionVisitor, ComparisonOperandVisitor  {
	public void visit(AndExpression expression) {
		expression.left().accept(this);
		expression.right().accept(this);
	}

	public void visit(BoolConstExpression expression) {
	}

	public void visit(OrExpression expression) {
		expression.left().accept(this);
		expression.right().accept(this);
	}

	public void visit(ComparisonExpression expression) {
		expression.left().accept(this);
		expression.right().accept(this);
	}

	public void visit(NotExpression expression) {
		expression.expr().accept(this);
	}
	
	public void visit(ArithmeticExpression operand) {
		operand.left().accept(this);
		operand.right().accept(this);
	}

	public void visit(ConstValue operand) {
	}

	public void visit(FieldValue operand) {
		operand.parent().accept(this);
	}

	public void visit(CandidateFieldRoot root) {
	}

	public void visit(PredicateFieldRoot root) {
	}

	public void visit(StaticFieldRoot root) {
	}

	public void visit(ArrayAccessValue operand) {
		operand.parent().accept(this);
		operand.index().accept(this);
	}

	public void visit(MethodCallValue value) {
		value.parent().accept(this);
		visitArgs(value);
	}

	protected void visitArgs(MethodCallValue value) {
		ComparisonOperand[] args = value.args();
		for (int i=0; i<args.length; ++i) {
			args[i].accept(this);
		}
	}
}
