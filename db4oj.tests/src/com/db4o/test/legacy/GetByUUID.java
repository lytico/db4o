/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.query.*;
import com.db4o.test.*;

public class GetByUUID {
    
    public String name;
    
    public GetByUUID(){
    }
    
    public GetByUUID(String name){
        this.name = name;
    }
    
    public void configure(){
        Db4o.configure().objectClass(this).generateUUIDs(true);
    }
    
    public void store(){
        Test.deleteAllInstances(GetByUUID.class);
        Test.store(new GetByUUID("one"));
        Test.store(new GetByUUID("two"));
    }
    
    public void test(){
        Hashtable4 ht = new Hashtable4(2);
        ExtObjectContainer oc = Test.objectContainer();
        Query q = Test.query();
        q.constrain(GetByUUID.class);
        ObjectSet objectSet = q.execute();
        while(objectSet.hasNext()){
            GetByUUID gbu = (GetByUUID)objectSet.next();
            Db4oUUID uuid = oc.getObjectInfo(gbu).getUUID();
            GetByUUID gbu2 =  (GetByUUID)oc.getByUUID(uuid);
            Test.ensure(gbu == gbu2);
            ht.put(gbu.name, uuid);
        }
        Test.reOpenServer();
        oc = Test.objectContainer();
        q = Test.query();
        q.constrain(GetByUUID.class);
        objectSet = q.execute();
        while(objectSet.hasNext()){
            GetByUUID gbu = (GetByUUID)objectSet.next();
            Db4oUUID uuid = (Db4oUUID)ht.get(gbu.name);
            GetByUUID gbu2 =  (GetByUUID)oc.getByUUID(uuid);
            Test.ensure(gbu == gbu2);
        }
    }
}
