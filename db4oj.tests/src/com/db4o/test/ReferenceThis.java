/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;


public class ReferenceThis {
    
    public ReferenceThis self;
    
    public void storeOne(){
        self = this;
    }
    
    public void testOne(){
        Test.ensure(self == this);
    }

}
