/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Iterator4JdkIteratorTestCase implements TestCase{

    public static void main(String[] arguments) {
        new ConsoleTestRunner(Iterator4JdkIteratorTestCase.class).run();
    }
    
    public void test(){
        Collection4 collection = new Collection4();
        Object[] content = new String[]{"one", "two", "three"};
        for (int i = 0; i < content.length; i++) {
            collection.add(content[i]);    
        }
        Iterator iterator = new Iterator4JdkIterator(collection.iterator());
        IteratorAssert.areEqual(content, iterator); 
    }

}
