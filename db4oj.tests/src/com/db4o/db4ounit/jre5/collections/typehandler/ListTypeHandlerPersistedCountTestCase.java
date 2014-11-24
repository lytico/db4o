/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections.typehandler;

import java.util.*;

import com.db4o.config.*;
import com.db4o.typehandlers.*;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ListTypeHandlerPersistedCountTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new ListTypeHandlerPersistedCountTestCase().runAll();
    }
    
    public static class TypedItem {
        
        ArrayList list;
        
    }
    
    public static class InterfaceItem {
        
        List list;
        
    }
    
    public static class UntypedItem {
        
        Object list;
        
    }
    
    protected void configure(Configuration config) throws Exception {
        config.registerTypeHandler(
            new SingleClassTypeHandlerPredicate(ArrayList.class), 
            new CollectionTypeHandler());
    }
    
    public void testTypedItem(){
        TypedItem typedItem = new TypedItem();
        typedItem.list = new ArrayList();
        store(typedItem);
        Db4oAssert.persistedCount(1, ArrayList.class);
    }
    
    public void testInterFaceItem(){
        InterfaceItem interfaceItem = new InterfaceItem();
        interfaceItem.list = new ArrayList();
        store(interfaceItem);
        Db4oAssert.persistedCount(1, ArrayList.class);
    }
    
    public void testUntypedItem(){
        UntypedItem untypedItem = new UntypedItem();
        untypedItem.list = new ArrayList();
        store(untypedItem);
        Db4oAssert.persistedCount(1, ArrayList.class);
    }
    
}
