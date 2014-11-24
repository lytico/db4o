/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class HandlerVersionRegistry {
    
    private final HandlerRegistry _registry;
    
    private final Hashtable4 _versions = new Hashtable4();
    
    public HandlerVersionRegistry(HandlerRegistry registry){
        _registry = registry;
    }

    public void put(TypeHandler4 handler, int version, TypeHandler4 replacement) {
        _versions.put(new HandlerVersionKey(handler, version), replacement);
    }

    public TypeHandler4 correctHandlerVersion(final TypeHandler4 originalHandler, final int version) {
        if(version >= HandlerRegistry.HANDLER_VERSION){
            return originalHandler;
        }
        if(originalHandler == null){
        	return null;  // HandlerVersionKey with null key will throw NPE.
        }
        TypeHandler4 replacement = (TypeHandler4) _versions.get(new HandlerVersionKey(genericTemplate(originalHandler), version));
        if(replacement == null){
            return correctHandlerVersion(originalHandler, version + 1);    
        }
        if(replacement instanceof VersionedTypeHandler){
            return (TypeHandler4) ((VersionedTypeHandler)replacement).deepClone(new TypeHandlerCloneContext(_registry, originalHandler,  version));
        };
        return replacement;
    }

    private TypeHandler4 genericTemplate(final TypeHandler4 handler) {
        if (handler instanceof VersionedTypeHandler){
            return ((VersionedTypeHandler)handler).unversionedTemplate(); 
        }
        return handler;
    }
    
    private class HandlerVersionKey {
        
        private final TypeHandler4 _handler;
        
        private final int _version;
        
        public HandlerVersionKey(TypeHandler4 handler, int version){
            _handler = handler;
            _version = version;
        }

        public int hashCode() {
            return _handler.hashCode() + _version * 4271;
        }

        public boolean equals(Object obj) {
            HandlerVersionKey other = (HandlerVersionKey) obj;
            return _handler.equals(other._handler) && _version == other._version;
        }

    }
    
}
