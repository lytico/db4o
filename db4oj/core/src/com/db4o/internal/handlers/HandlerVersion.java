/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;


/**
 * @exclude
 */
public class HandlerVersion {
    
    public final int _number;
    
    public static final HandlerVersion INVALID = new HandlerVersion(-1);
    
    public HandlerVersion(int number){
        _number = number;
    }
    
    public boolean equals(Object obj) {
        if(this == obj){
            return true;
        }
        return ((HandlerVersion)obj)._number == _number;
    }
    
}
