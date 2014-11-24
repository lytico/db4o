/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.pending;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class IndexOnParentClass {
    
    public String name;
    
    public void configure(){
        Db4o.configure().objectClass(this).objectField("name").indexed(true);
    }
    
    public void store(){
        IndexOnParentClass p = new IndexOnParentClass();
        p.name = "all";
        Test.store(p);
        p = new ChildClass();
        p.name = "all";
        Test.store(p);
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(ChildClass.class);
        q.descend("name").constrain("all");
        int size = q.execute().size();
        System.out.println(size);
        Test.ensure(size == 1);
    }
    
    public static class ChildClass extends IndexOnParentClass{
        
    }
}
