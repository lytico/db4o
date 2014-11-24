/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


public class SubTypePredicate implements Predicate4 {
    
    private final Class _class;
    
    public SubTypePredicate(Class clazz){
        _class = clazz;
    }

    public boolean match(Object candidate) {
        return _class.isInstance(candidate);
    }

}
