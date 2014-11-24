/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.build;

import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.field.*;

public class MockComparisonExpressionBuilder {
	private int id=0;
	
	public ComparisonExpression build() {
		id++;
		return new ComparisonExpression(new FieldValue(CandidateFieldRoot.INSTANCE,"a"+id),new ConstValue(String.valueOf(id)),ComparisonOperator.EQUALS);
	}
}
