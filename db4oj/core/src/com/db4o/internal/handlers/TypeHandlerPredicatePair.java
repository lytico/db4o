/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class TypeHandlerPredicatePair {
    
    public final TypeHandlerPredicate _predicate;
    
    public final TypeHandler4 _typeHandler;

    public TypeHandlerPredicatePair(TypeHandlerPredicate predicate, TypeHandler4 typeHandler) {
        _predicate = predicate;
        _typeHandler = typeHandler;
    }
    
}
