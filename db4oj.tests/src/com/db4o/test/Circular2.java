/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

public class Circular2 {
    
    public Hashtable ht;
    
    public void storeOne(){
        Test.objectContainer().configure().updateDepth(Integer.MAX_VALUE);
        ht = new Hashtable();
        C2C c2c = new C2C();
        c2c.parent = this;
        ht.put("test", c2c);
    }
    
    public void testOne(){
        C2C c2c = (C2C)ht.get("test");
        Test.ensure(c2c.parent == this);
        Test.objectContainer().configure().updateDepth(5);
    }
    
    public static class C2C{
        public Circular2 parent;
    }
}
