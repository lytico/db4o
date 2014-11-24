/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;


public class CandidateFieldRoot extends ComparisonOperandRoot {
	public final static CandidateFieldRoot INSTANCE=new CandidateFieldRoot();
	
	private CandidateFieldRoot() {}
	
	public String toString() {
		return "CANDIDATE";
	}

	public void accept(ComparisonOperandVisitor visitor) {
		visitor.visit(this);
	}
}
