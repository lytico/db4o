/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.aliases;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Parent1 {
    
    public Child1 child;
    
    public Parent1(){
    }
    
    public Parent1(Child1 child){
        this.child = child;
    }


}
