/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr;

import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.field.*;

import db4ounit.*;

public class ExpressionUnitTest implements TestCase {
	public void testEqualsHashCodeFieldValue() {
		FieldValue fieldValue = new FieldValue(PredicateFieldRoot.INSTANCE,"a");
		assertEqualsHashCode(fieldValue,new FieldValue(PredicateFieldRoot.INSTANCE,"a"));
		assertNotEquals(fieldValue,new FieldValue(PredicateFieldRoot.INSTANCE,"b"));
	}
	
	public void testEqualsHashCodeConst() {
		BoolConstExpression expr = BoolConstExpression.TRUE;
		assertEqualsHashCode(expr,BoolConstExpression.TRUE);
		assertNotEquals(expr,new FieldValue(PredicateFieldRoot.INSTANCE,"b"));
	}

	public void testEqualsHashCodeNot() {
		NotExpression expr = new NotExpression(BoolConstExpression.TRUE);
		assertEqualsHashCode(expr,new NotExpression(BoolConstExpression.TRUE));
		assertNotEquals(expr,new NotExpression(BoolConstExpression.FALSE));
	}

	public void testEqualsHashCodeAnd() {
		AndExpression expr = new AndExpression(BoolConstExpression.TRUE,BoolConstExpression.FALSE);
		assertEqualsHashCode(expr,new AndExpression(BoolConstExpression.TRUE,BoolConstExpression.FALSE));
		assertNotEquals(expr,new AndExpression(BoolConstExpression.FALSE,BoolConstExpression.FALSE));
		assertNotEquals(expr,new AndExpression(BoolConstExpression.TRUE,BoolConstExpression.TRUE));
		assertEqualsHashCode(expr,new AndExpression(BoolConstExpression.FALSE,BoolConstExpression.TRUE));
	}

	public void testEqualsHashCodeOr() {
		OrExpression expr = new OrExpression(BoolConstExpression.TRUE,BoolConstExpression.FALSE);
		assertEqualsHashCode(expr,new OrExpression(BoolConstExpression.TRUE,BoolConstExpression.FALSE));
		assertNotEquals(expr,new OrExpression(BoolConstExpression.FALSE,BoolConstExpression.FALSE));
		assertNotEquals(expr,new OrExpression(BoolConstExpression.TRUE,BoolConstExpression.TRUE));
		assertEqualsHashCode(expr,new OrExpression(BoolConstExpression.FALSE,BoolConstExpression.TRUE));
	}

	public void testEqualsHashCodeComparison() {
		FieldValue[] fieldVals={new FieldValue(PredicateFieldRoot.INSTANCE,"A"),new FieldValue(CandidateFieldRoot.INSTANCE,"B")};
		ConstValue[] constVals={new ConstValue("X"),new ConstValue("Y")};
		ComparisonExpression expr = new ComparisonExpression(fieldVals[0],constVals[0],ComparisonOperator.EQUALS);
		assertEqualsHashCode(expr,new ComparisonExpression(fieldVals[0],constVals[0],ComparisonOperator.EQUALS));
		assertNotEquals(expr,new ComparisonExpression(fieldVals[1],constVals[0],ComparisonOperator.EQUALS));
		assertNotEquals(expr,new ComparisonExpression(fieldVals[0],constVals[1],ComparisonOperator.EQUALS));
		assertNotEquals(expr,new ComparisonExpression(fieldVals[0],constVals[0],ComparisonOperator.SMALLER));
	}

	private void assertEqualsHashCode(Object obj,Object same) {
		Assert.isTrue(obj.equals(obj));
		Assert.isTrue(obj.equals(same));
		Assert.isTrue(same.equals(obj));
		Assert.isFalse(obj.equals(null));
		Assert.isFalse(obj.equals(new Object()));
		Assert.areEqual(obj.hashCode(),same.hashCode());
	}

	private void assertNotEquals(Object obj,Object other) {
		Assert.isFalse(obj.equals(other));
		Assert.isFalse(other.equals(obj));
	}
}
