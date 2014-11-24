/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;


public class CallbackCanDelete {
    
    public String _name;
    
    public CallbackCanDelete _next;
    
    public CallbackCanDelete() {
    
    }

    public CallbackCanDelete(String name_, CallbackCanDelete next_) {
        _name = name_;
        _next = next_;
    }
    
    public void storeOne(){
        Test.deleteAllInstances(this);
        _name = "p1";
        _next = new CallbackCanDelete("c1", null);
    }
    
    public void test(){
        ObjectContainer oc = Test.objectContainer();
        ObjectSet objectSet = oc.queryByExample(new CallbackCanDelete("p1", null));
        CallbackCanDelete ccd = (CallbackCanDelete) objectSet.next();
        oc.deactivate(ccd, Integer.MAX_VALUE);
        oc.delete(ccd);
    }
    
    
    public boolean objectCanDelete(ObjectContainer container){
        container.activate(this, Integer.MAX_VALUE);
        Test.ensure(_name.equals("p1"));
        Test.ensure(_next != null);
        return true;
    }
    
}
