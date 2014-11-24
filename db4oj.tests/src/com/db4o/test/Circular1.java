/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.query.*;

/**
 * 
 */
public class Circular1 {
    
    public void store(){
        Test.store(new C1C());
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(C1C.class);
        Test.ensure(q.execute().size() > 0);
    }
    
    public static class C1P{
        C1C c;
    }
    
    public static class C1C extends C1P{
    }
}
