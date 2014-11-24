/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class QueryForStringKeyInMap {
    
    String name;
    Map map;
    
    public void store(){
        store1("one");
        store1("two");
        store1("three");
    }
    
    private void store1(String key){
        ExtObjectContainer oc = Test.objectContainer();
        QueryForStringKeyInMap holder = new QueryForStringKeyInMap();
        oc.store(holder);
        holder.map = new HashMap(1);
        holder.map.put("somethingelse", "somethingelse");
        holder.map.put(key, key);
        holder.name = key;
    }
    
    public void test(){
        t1("one");
        t1("two");
        t1("three");
    }
    
    private void t1(String key){
        Query q = Test.query();
        q.constrain(QueryForStringKeyInMap.class);
        q.descend("map").constrain(key);
        ObjectSet objectSet = q.execute();
        Test.ensure(objectSet.size() == 1);
        QueryForStringKeyInMap holder = (QueryForStringKeyInMap)objectSet.next();
        Test.ensure(holder.map.get(key).equals(key));
        Test.ensure(holder.name.equals(key));
    }

}
