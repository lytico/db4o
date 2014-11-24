/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;


public class GetAllSoda {
    
    public String name;
    
    
    public GetAllSoda() {
        
    }
    
    public GetAllSoda(String name) {
        this.name = name;
    }
    
    public void store(){
        Test.store(new GetAllSoda("one"));
        Test.store(new GetAllSoda("two"));
    }
    
    public void testQuery() {
        Query q = Test.query();
        ObjectSet objectSet = q.execute();
        Test.ensure(objectSet.size() >= 2);
        int i = 0;
        while(objectSet.hasNext()){
            Object obj = objectSet.next();
            Test.ensure(obj != null);
            i ++;
        }
        Test.ensure(i >= 2);
    }

}
