/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.jdk5;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Jdk5DeleteEnum {
    
    Jdk5Enum a;
    
    public void configure(){
        Db4o.configure().objectClass(this).cascadeOnDelete(true);
    }
    
    public void store(){
        for (int i = 0; i < 2; i++) {
            Jdk5DeleteEnum jde = new Jdk5DeleteEnum();
            jde.a = Jdk5Enum.A;
            Test.store(jde);
        }
    }
    
    public void test(){
        Jdk5DeleteEnum jde = queryOne(); 
        Test.delete(jde);
        Test.reOpen();
        jde = queryOne();
        Test.ensure(jde.a == Jdk5Enum.A);
    }
    
    private Jdk5DeleteEnum queryOne(){
        Query q = Test.query();
        q.constrain(Jdk5DeleteEnum.class);
        ObjectSet objectSet = q.execute();
        return (Jdk5DeleteEnum)objectSet.next();
    }
    
}
