/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;


/**
 * 
 */
public class NoInstanceStored {
    
    public String name;
    
    public void test(){
        Query q = Test.query();
        q.constrain(NoInstanceStored.class);
        q.descend("name").constrain("hi");
        ObjectSet objectSet = q.execute();
        Test.ensure(objectSet.size() == 0);
    }

}
