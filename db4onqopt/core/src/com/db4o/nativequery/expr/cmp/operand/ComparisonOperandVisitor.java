/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.nativequery.expr.cmp.operand;


public interface ComparisonOperandVisitor {
	void visit(ArithmeticExpression operand);
	void visit(ConstValue operand);
	void visit(FieldValue operand);
	void visit(CandidateFieldRoot root);
	void visit(PredicateFieldRoot root);
	void visit(StaticFieldRoot root);
	void visit(ArrayAccessValue operand);
	void visit(MethodCallValue value);
}