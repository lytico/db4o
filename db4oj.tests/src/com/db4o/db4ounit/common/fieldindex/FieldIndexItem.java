/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;


public class FieldIndexItem implements HasFoo {
    
    public int foo;
    
    public FieldIndexItem() {
    }
    
    public FieldIndexItem(int foo_) {
        foo = foo_;
    }
    
    public int getFoo() {
    	return foo;
    }
}
