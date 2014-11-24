/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.interfaces;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class QueryByInterfaceTestCase extends AbstractDb4oTestCase {
	
	protected void store(){
        store(new ThreeSomeParent());
        store(new ThreeSomeLeftChild());
        store(new ThreeSomeRightChild());
    }
    
    public void test(){
        Query q = newQuery(ThreeSomeInterface.class);
        Assert.areEqual(2, q.execute().size());
    }

}
