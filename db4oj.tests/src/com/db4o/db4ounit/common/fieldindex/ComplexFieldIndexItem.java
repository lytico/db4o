/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.fieldindex;

public class ComplexFieldIndexItem implements HasFoo {
	public int foo;
	public int bar;
	public ComplexFieldIndexItem child;
	
	public ComplexFieldIndexItem() {
	}
	
	public ComplexFieldIndexItem(int foo_, int bar_, ComplexFieldIndexItem child_) {
		foo = foo_;
		bar = bar_;
		child = child_;
	}
	
	public int getFoo() {
		return foo;
	}
}
