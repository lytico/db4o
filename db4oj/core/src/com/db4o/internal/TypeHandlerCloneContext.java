/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class TypeHandlerCloneContext {
    
    private final HandlerRegistry handlerRegistry;
    
    public final TypeHandler4 original;
    
    private final int version;

    public TypeHandlerCloneContext(HandlerRegistry handlerRegistry_, TypeHandler4 original_, int version_) {
        handlerRegistry = handlerRegistry_;
        original = original_;
        version = version_;
    }
    
    public TypeHandler4 correctHandlerVersion(TypeHandler4 typeHandler){
        return handlerRegistry.correctHandlerVersion(typeHandler, version);
    }
    
    
}
