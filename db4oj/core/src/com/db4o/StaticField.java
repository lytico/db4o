/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 * @persistent
 */
public class StaticField implements Internal4{
    public String name;
    public Object value;
    
    public StaticField(){
    }
    
    public StaticField(String name_, Object value_){
        name = name_;
        value = value_;
    }
}
