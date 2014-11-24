/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.query.*;


/**
 * 
 */
public class UnknownClass {
    
    public void store(){
        Test.store(new Atom());
    }
    
    public void test(){
        Query q = Test.query();
        // q.constrain(UnknownClass.class);
        Test.ensure(q.execute().size() == 0);
    }

}
