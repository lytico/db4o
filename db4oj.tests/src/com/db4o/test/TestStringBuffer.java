/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TestStringBuffer {
    
    public StringBuffer sb;
    
    public TestStringBuffer(){
        
    }
    
    public TestStringBuffer(String str){
        sb = new StringBuffer(str);
    }
    
    public void store(){
        Test.deleteAllInstances(this);
//        Test.store(new TestStringBuffer("Aloha"));
//        Test.store(new TestStringBuffer("Yohaa"));
        Test.store(new TestStringBuffer("Aloe Vera"));
//        Test.store(new TestStringBuffer("Store Aloha"));
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(new TestStringBuffer("Vera"));
        q.descend("sb").constraints().contains();
        ObjectSet os = q.execute();
        Test.ensure(os.size() == 1);
        TestStringBuffer tbs = (TestStringBuffer)os.next();
        Test.ensure(tbs.sb.toString().equals("Aloe Vera"));
        
//        q = Test.query();
//        q.constrain(new TestStringBuffer("Yohaa"));
//        os = q.execute();
//        Test.ensure(os.size() == 1);
//        tbs = (TestStringBuffer)os.next();
//        Test.ensure(tbs.sb.toString().equals("Yohaa"));
    }
}
