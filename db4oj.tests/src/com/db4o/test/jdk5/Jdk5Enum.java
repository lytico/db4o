/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.jdk5;



/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public enum Jdk5Enum {
    
    A("A"),
    B("B");
    
    private String type;
    private int count;

    private Jdk5Enum(String type) {
       this.type = type;
       this.count=0;
    }

    public String getType() {
       return "type "+type;
    }
    
    public int getCount() {
    	return count;
    }
    
    public void incCount() {
    	count++;
    }
    
    public void reset(){
        count = 0;
    }
 }
