/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;


public class MultiLevelIndex {
    
    public MultiLevelIndex _child;
    public int _i;
    public int _level;
    
    public void configure(){
        Db4o.configure().objectClass(this).objectField("_child").indexed(true);
        Db4o.configure().objectClass(this).objectField("_i").indexed(true);
    }
    
    public void store(){
        Test.deleteAllInstances(this);
        store1(3);
        store1(2);
        store1(5);
        store1(1);
        for (int i = 6; i < 103; i++) {
            store1(i);
        }
    }
    
    private void store1(int val){
        MultiLevelIndex root = new MultiLevelIndex();
        root._i = val;
        root._child = new MultiLevelIndex();
        root._child._level = 1;
        root._child._i = - val ;
        Test.store(root);
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(MultiLevelIndex.class);
        q.descend("_child").descend("_i").constrain(new Integer(- 102));
        long start = System.currentTimeMillis();
        ObjectSet objectSet = q.execute();
        long stop = System.currentTimeMillis();
        long duration = stop - start;
        System.out.println("MultiLevelIndex time: " + duration + "ms");

        
        Test.ensure(objectSet.size() == 1);
        MultiLevelIndex mli = (MultiLevelIndex)objectSet.next();
        Test.ensure(mli._i == 102);
        
    }
    
    
    

}
