/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.aliases;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.test.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ClassAliasesInheritance {
    
    public void test(){
        
        Test.store(new Parent1(new Child1()));
        
        ObjectContainer container = Test.reOpen();
        container.ext().configure().addAlias(
            new TypeAlias("com.db4o.test.aliases.Parent1",
                        "com.db4o.test.aliases.Parent2"));
        container.ext().configure().addAlias(
            new TypeAlias("com.db4o.test.aliases.Child1",
                        "com.db4o.test.aliases.Child2"));
        
        ObjectSet os = container.query(Parent2.class);
        
        Test.ensure(os.size() > 0);
        
        Parent2 p2 = (Parent2)os.next();
        
        Test.ensure(p2.child != null);
    }

}
