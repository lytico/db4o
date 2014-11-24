/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.ordered;
import com.db4o.query.*;


public class STOStringTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	public String foo;

    public STOStringTestCase() {
    }

    public STOStringTestCase(String str) {
        this.foo = str;
    }
    
    @Override
    public String toString() {
    	return foo;
    }

    public Object[] createData() {
        return new Object[] {
            new STOStringTestCase(null),
            new STOStringTestCase("bbb"),
            new STOStringTestCase("dod"),
            new STOStringTestCase("aaa"),
            new STOStringTestCase("Xbb"),
            new STOStringTestCase("bbq")};
    }

    public void testAscending() {
        Query q = newQuery();
        q.constrain(STOStringTestCase.class);
        q.descend("foo").orderAscending();
        
        expectOrdered(q, new int[] { 0, 4, 3, 1, 5, 2 });
    }

    public void testDescending() {
        Query q = newQuery();
        q.constrain(STOStringTestCase.class);
        q.descend("foo").orderDescending();
        
        expectOrdered(q, new int[] { 2, 5, 1, 3, 4, 0 });
    }

    public void testAscendingLike() {
        Query q = newQuery();
        q.constrain(STOStringTestCase.class);
        Query qStr = q.descend("foo");
        qStr.constrain("b").like();
        qStr.orderAscending();
        
        expectOrdered(q, new int[] { 4, 1, 5 });
    }

    public void testDescendingContains() {
        Query q = newQuery();
        q.constrain(STOStringTestCase.class);
        Query qStr = q.descend("foo");
        qStr.constrain("b").contains();
        qStr.orderDescending();
        
        expectOrdered(q, new int[] { 5, 1, 4 });
    }
}
