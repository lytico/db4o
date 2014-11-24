/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;


public class TestDescend {
    
    public TestDescend _child;
    
    public String _name;
    
    public TestDescend(){
        
    }
    
    public TestDescend(TestDescend child, String name){
        _child = child;
        _name = name;
    }
    
    public void storeOne(){
        _child = new TestDescend(new TestDescend(new TestDescend(null, "3"), "2"), "1");
        _name = "0";
    }
    
    public void test(){
        if(Test.isClientServer()){
            return;
        }
        Query q = Test.query();
        q.constrain(this.getClass());
        q.descend("_name").constrain("0");
        ObjectSet objectSet = q.execute();
        TestDescend res = (TestDescend)objectSet.next();
        
        ExtObjectContainer oc = Test.objectContainer();
        
        Object obj = oc.descend(res, new String[]{"_name"});
        Test.ensure(obj.equals("0"));
        
        obj = oc.descend(res, new String[]{"_child", "_child", "_name"});
        Test.ensure(obj.equals("2"));
        
        obj = oc.descend(res, new String[]{"_child", "_child", "_child", "_name"});
        Test.ensure(obj.equals("3"));
        
        obj = oc.descend(res, new String[]{"_child", "CRAP", "_child", "_name"});
        Test.ensure(obj == null);
    }

}
