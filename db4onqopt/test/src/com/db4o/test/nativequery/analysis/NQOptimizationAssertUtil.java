/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery.analysis;

import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.operand.*;

import db4ounit.*;

public final class NQOptimizationAssertUtil {

	public static void assertComparison(Expression expr, String[] fieldNames, Object value, ComparisonOperator op, boolean negated) {
		if(negated) {
			NotExpression notExpr=(NotExpression)expr;
			expr=notExpr.expr();
		}
		ComparisonExpression cmpExpr=(ComparisonExpression)expr;
		Assert.areEqual(op, cmpExpr.op());
		ComparisonOperand curop=cmpExpr.left();
		for(int foundFieldIdx=fieldNames.length-1;foundFieldIdx>=0;foundFieldIdx--) {
			FieldValue fieldValue=(FieldValue)curop;
			Assert.areEqual(fieldNames[foundFieldIdx], fieldValue.fieldName());
			curop=fieldValue.parent();
		}
		Assert.areEqual(CandidateFieldRoot.INSTANCE,curop);
		ComparisonOperand right = cmpExpr.right();
		if(right instanceof ConstValue) {
			Assert.areEqual(value, ((ConstValue) right).value());
			return;
		}
		Assert.areEqual(value,right);
	}

	private NQOptimizationAssertUtil() {
	}

}
