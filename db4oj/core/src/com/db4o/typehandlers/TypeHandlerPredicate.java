/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.typehandlers;

import com.db4o.reflect.*;


/**
 * Predicate to be able to select if a specific TypeHandler is
 * applicable for a specific Type.
 */
public interface TypeHandlerPredicate {
    
    /**
     * return true if a TypeHandler is to be used for a specific
     * Type 
     * @param classReflector the Type passed by db4o that is to
     * be tested by this predicate.
     * @return true if the TypeHandler is to be used for a specific
     * Type.
     */
    public boolean match(ReflectClass classReflector);

}
