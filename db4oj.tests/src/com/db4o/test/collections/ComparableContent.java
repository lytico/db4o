/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.collections;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ComparableContent implements Comparable{
    
    public String _name;
    
    public ComparableContent _child;
    
    public ComparableContent(){
        
    }
    
    public ComparableContent(String name){
        _name = name;
        _child = new ComparableContent();
    }

    public int compareTo(Object o) {
        if(_name == null){
            throw new NullPointerException();
        }
        if(_child == null){
            throw new NullPointerException();
        }
        ComparableContent other = (ComparableContent) o;
        if(other._child == null){
            throw new NullPointerException();
        }
        return other._name.compareTo(_name);
    }
    
    public boolean equals(Object obj) {
        ComparableContent other = (ComparableContent) obj;
        return other._name.equals(_name);
    }
    
}
