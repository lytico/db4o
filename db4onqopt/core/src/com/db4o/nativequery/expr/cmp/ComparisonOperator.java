/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp;

// TODO: switch to individual classes and visitor dispatch?
public final class ComparisonOperator {
	public final static int EQUALS_ID=0;
	public final static int SMALLER_ID=1;
	public final static int GREATER_ID=2;
	public final static int CONTAINS_ID=3;
	public final static int STARTSWITH_ID=4;
	public final static int ENDSWITH_ID=5;
	public final static int IDENTITY_ID=6;

	public final static ComparisonOperator VALUE_EQUALITY=new ComparisonOperator(EQUALS_ID,"==", true);
	public final static ComparisonOperator SMALLER=new ComparisonOperator(SMALLER_ID,"<", false);
	public final static ComparisonOperator GREATER=new ComparisonOperator(GREATER_ID,">", false);
	public final static ComparisonOperator CONTAINS=new ComparisonOperator(CONTAINS_ID,"<CONTAINS>", false);
	public final static ComparisonOperator STARTS_WITH=new ComparisonOperator(STARTSWITH_ID,"<STARTSWITH>", false);
	public final static ComparisonOperator ENDS_WITH=new ComparisonOperator(ENDSWITH_ID,"<ENDSWITH>", false);
	public final static ComparisonOperator REFERENCE_EQUALITY=new ComparisonOperator(IDENTITY_ID,"===", true);

	private int _id;
	private String _op;
	private boolean _symmetric;
	
	private ComparisonOperator(int id, String op, boolean symmetric) {
		_id=id;
		_op=op;
		_symmetric=symmetric;
	}
	
	public int id() {
		return _id;
	}
	
	public String toString() {
		return _op;
	}
	
	public boolean isSymmetric() {
		return _symmetric;
	}
}
