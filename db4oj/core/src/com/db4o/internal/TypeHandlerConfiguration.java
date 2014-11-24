/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public abstract class TypeHandlerConfiguration {
    
    protected final Config4Impl _config;
    
    private TypeHandler4 _listTypeHandler;
    
    private TypeHandler4 _mapTypeHandler;
    
    public abstract void apply();
    
    public TypeHandlerConfiguration(Config4Impl config){
        _config = config;
    }
    
    protected void listTypeHandler(TypeHandler4 listTypeHandler){
    	_listTypeHandler = listTypeHandler;
    }
    
    protected void mapTypeHandler(TypeHandler4 mapTypehandler){
    	_mapTypeHandler = mapTypehandler;
    }
    
    protected void registerCollection(Class clazz){
        registerListTypeHandlerFor(clazz);    
    }
    
    protected void registerMap(Class clazz){
        registerMapTypeHandlerFor(clazz);    
    }
    
    protected void ignoreFieldsOn(Class clazz){
    	registerTypeHandlerFor(clazz, IgnoreFieldsTypeHandler.INSTANCE);
    }
    
    protected void ignoreFieldsOn(String className){
    	registerTypeHandlerFor(className, IgnoreFieldsTypeHandler.INSTANCE);
    }
    
    private void registerListTypeHandlerFor(Class clazz){
        registerTypeHandlerFor(clazz, _listTypeHandler);
    }
    
    private void registerMapTypeHandlerFor(Class clazz){
        registerTypeHandlerFor(clazz, _mapTypeHandler);
    }
    
    protected void registerTypeHandlerFor(Class clazz, TypeHandler4 typeHandler){
        _config.registerTypeHandler(new SingleClassTypeHandlerPredicate(clazz), typeHandler);
    }
    
    protected void registerTypeHandlerFor(String className, TypeHandler4 typeHandler){
        _config.registerTypeHandler(new SingleNamedClassTypeHandlerPredicate(className), typeHandler);
    }


}
