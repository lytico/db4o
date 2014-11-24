/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery.expr.build;

import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.operand.*;
import com.db4o.test.nativequery.mocks.*;

public class MockComparisonExpressionBuilder {
	private int id=0;
	
	public ComparisonExpression build() {
		id++;
		return new ComparisonExpression(
					new FieldValue(
						CandidateFieldRoot.INSTANCE,
						new MockFieldRef("a"+id)),
					new ConstValue(String.valueOf(id)),
					ComparisonOperator.VALUE_EQUALITY);
	}
}
