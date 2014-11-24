/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;


public class UuidAware {
    
    public String name;
    
    private transient long uuidLongPart;

    private transient byte[] uuidSignaturePart;
    
    
    public UuidAware() {
     
    }
    
    public UuidAware(String name) {
        this.name = name;
    }

    public void configure(){
        Db4o.configure().objectClass(this).generateUUIDs(true);
    }
    
    public void store(){
        Test.objectContainer().store(new UuidAware("one"));
        Test.objectContainer().store(new UuidAware("two"));
    }
    
    public void test(){
        ExtObjectContainer oc = Test.objectContainer();

        UuidAware ua = queryName("one");
        ua.checkUUID(oc);
        
        ua = queryName("two");
        ua.checkUUID(oc);
    }
    
    private UuidAware queryName(String name){
        Query q = Test.query();
        q.constrain(getClass());
        q.descend(name);
        return (UuidAware) q.execute().next();
    }
    
    private void checkUUID(ExtObjectContainer oc){
        Db4oUUID uuid = new Db4oUUID(uuidLongPart, uuidSignaturePart);
        Test.ensure(oc.getByUUID(uuid) == this);
    }
    
    public void objectOnActivate(ObjectContainer oc){
        setUuidFields(oc);
    }
    
    public void objectOnNew(ObjectContainer oc){
        setUuidFields(oc);
    }
    
    private void setUuidFields(ObjectContainer oc){
        ObjectInfo info = oc.ext().getObjectInfo(this);
        Db4oUUID uuid = info.getUUID();
        uuidLongPart = uuid.getLongPart();
        uuidSignaturePart = uuid.getSignaturePart();
    }
    

}
