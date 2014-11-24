/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

public class ServerUpdateSpeed {
    
    Atom child;
    String name;
    
    public void configure(){
        Db4o.configure().updateDepth(Integer.MAX_VALUE);
    }
    
    public void storeOne(){
        child = new Atom();
        name = "hi";
        child.name = "hitoo";
    }
    
    public void testOne(){
        name = "haitoo";
        child.name = "wasup";
        Test.store(this);
    }

}
