/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

public class ObjectContainerIsTransient {
    
    public String foo;
    public ObjectContainer objectContainer;
    
    public void storeOne(){
        objectContainer = Test.objectContainer();
        foo = "foo";
    }
    
    public void testOne(){
        Test.ensure(objectContainer == null);
        Test.ensure(foo.equals("foo"));
    }
}
