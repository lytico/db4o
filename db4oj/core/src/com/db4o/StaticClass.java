/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 * @persistent
 */
public class StaticClass implements Internal4{
    public String name;
    public StaticField[] fields;
    
    public StaticClass() {
    }
    
    public StaticClass(String name_, StaticField[] fields_) {
    	name = name_;
    	fields = fields_;
    }
}
