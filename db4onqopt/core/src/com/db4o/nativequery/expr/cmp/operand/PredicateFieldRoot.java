/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;


public class PredicateFieldRoot extends ComparisonOperandRoot {
	public final static PredicateFieldRoot INSTANCE=new PredicateFieldRoot();
	
	private PredicateFieldRoot() {}

	public String toString() {
		return "PREDICATE";
	}

	public void accept(ComparisonOperandVisitor visitor) {
		visitor.visit(this);
	}
}
