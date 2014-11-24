/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

// Why is this duplicated in jdk1.2/jdk5?

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ObjectSetAsList {
    
    String name;
    
    public ObjectSetAsList(){
    }
    
    public ObjectSetAsList(String name_){
        name = name_;
    }
    
    public void store(){
        Test.deleteAllInstances(this);
        Test.store(new ObjectSetAsList("one"));
        Test.store(new ObjectSetAsList("two"));
        Test.store(new ObjectSetAsList("three"));
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(ObjectSetAsList.class);
        List list = q.execute();
        Test.ensure(list.size() == 3);
        Iterator i = list.iterator();
        boolean found = false;
        while(i.hasNext()){
            ObjectSetAsList osil = (ObjectSetAsList)i.next();
            if(osil.name.equals("two")){
                found = true;
            }
        }
        Test.ensure(found);
    }

	public void testAccessOrder() {
		Query query=Test.query();
		query.constrain(getClass());
		ObjectSet result=query.execute();
		Test.ensureEquals(3,result.size());
		for(int i=0;i<3;i++) {
			Test.ensure(result.get(i) == result.next());
		}
		Test.ensure(!result.hasNext());
	}
}
