/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery.cats;

import com.db4o.*;
import com.db4o.query.*;


public abstract class SodaCatPredicate extends Predicate {
    
    private int _count;
    
    public void sodaQuery(ObjectContainer oc){
        Query q = oc.query();
        q.constrain(Cat.class);
        constrain(q);
        q.execute();
    }
    
    public abstract void constrain(Query q);
    
    public void setCount(int count){
        _count = count;
    }
    
    public int lower() {
        return _count / 2 - 1;
    }
    
    public int upper() {
        return _count / 2 + 1;
    }

    

}
