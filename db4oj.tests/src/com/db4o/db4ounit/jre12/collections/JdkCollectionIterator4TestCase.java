/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class JdkCollectionIterator4TestCase implements TestCase{
    
    public static void main(String[] arguments) {
        new ConsoleTestRunner(JdkCollectionIterator4TestCase.class).run();
    }
    
    public void test(){
        Collection collection = new ArrayList();
        Object[] content = new String[]{"one", "two", "three"};
        for (int i = 0; i < content.length; i++) {
            collection.add(content[i]);    
        }
        Iterator4 iterator = Iterators.iterator(collection);
        Iterator4Assert.areEqual(content, iterator); 
    }

}
