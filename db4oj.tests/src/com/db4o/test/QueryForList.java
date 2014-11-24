/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;


/**
 * 
 */
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class QueryForList {
    
    List _list;
    
    public void storeOne(){
        _list = new QueryForListArrayList();
        _list.add("hi");
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(List.class);
        ObjectSet objectSet = q.execute();
        int found = 0;
        while(objectSet.hasNext()){
            Object obj = objectSet.next();
            Test.ensure(obj instanceof List);
            List list = (List)obj;
            if(list instanceof QueryForListArrayList){
                found++;
                Test.ensure(list.get(0).equals("hi"));
            }
        }
        Test.ensure(found == 1);
    }
    
    static class QueryForListArrayList extends ArrayList{
    }

}
