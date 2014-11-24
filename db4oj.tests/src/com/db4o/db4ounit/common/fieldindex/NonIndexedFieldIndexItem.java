/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

/**
 * @exclude
 */
public class NonIndexedFieldIndexItem implements HasFoo {

	public int foo;
	
	public int indexed;
    
    public NonIndexedFieldIndexItem() {
    }
    
    public NonIndexedFieldIndexItem(int foo_) {
        foo = foo_;
        indexed = foo_;
    }
    
    public int getFoo() {
    	return foo;
    }

}
