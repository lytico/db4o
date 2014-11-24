/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.nonta;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
    
    public static void main(String[] args) {
        new AllTests().runAll();
    }

    protected Class[] testCases() {
        return new Class[] {
            NonTAArrayTestCase.class,
            NonTADateTestCase.class,
            NonTAIntTestCase.class,
            NonTALinkedListTestCase.class,
            NonTANArrayTestCase.class,
            NonTARefreshTestCase.class,
            NonTAStringTestCase.class,
        };
    }

}
