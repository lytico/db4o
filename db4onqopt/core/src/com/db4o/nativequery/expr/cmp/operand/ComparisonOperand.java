/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;

import com.db4o.nativequery.expr.*;


public interface ComparisonOperand extends ExpressionPart {
	void accept(ComparisonOperandVisitor visitor);

}
