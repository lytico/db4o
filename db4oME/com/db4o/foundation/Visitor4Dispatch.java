/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class Visitor4Dispatch implements Visitor4{
    
    public final Visitor4 _target;
    
    public Visitor4Dispatch(Visitor4 visitor){
        _target = visitor;
    }

    public void visit(Object a_object) {
        ((Visitor4)a_object).visit(_target);
    }
}
