/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.util.*;


/**
 * 
 */
public abstract class TestSuite {
    
    protected Vector _testSuites;
    
    public abstract Class[] tests();
    
    protected TestSuite suite(String name){
        try{
            Class clazz = Class.forName(name);
            if(clazz != null){
                TestSuite ts = (TestSuite)clazz.newInstance();
                return ts;
            }
        }catch(Exception e){
            
        }
        return null;
    }
    
    public boolean equals(Object obj){
        if(this == obj){
            return true;
        }
        if(obj == null){
            return false;
        }
        if(! (obj instanceof TestSuite)) {
            return false;
        }
        return obj.getClass() == this.getClass(); 
    }
    
    public void add(TestSuite suite){
        if(_testSuites == null){
            _testSuites = new Vector();
        }
        if(_testSuites.contains(suite)){
            return;
        }
        _testSuites.addElement(suite);
    }

}
