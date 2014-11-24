/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.ext.*;

/**
 * 
 */
public class DeepSet {
    
    public DeepSet child;
    public String name;
    
    public void storeOne(){
        name = "1";
        child = new DeepSet();
        child.name = "2";
        child.child = new DeepSet();
        child.child.name = "3";
    }
    
    public void test(){
        ExtObjectContainer oc = Test.objectContainer(); 
        name = "1";
        DeepSet ds = (DeepSet)oc.queryByExample(this).next();
        ds.name="11";
        ds.child.name = "12";
        oc.store(ds, 2);
        oc.deactivate(ds, Integer.MAX_VALUE);
        name = "11";
        ds = (DeepSet)oc.queryByExample(this).next();
        Test.ensure(ds.child.name.equals("12"));
        Test.ensure(ds.child.child.name.equals("3"));
    }

}
