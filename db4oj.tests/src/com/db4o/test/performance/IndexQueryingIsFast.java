/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.performance;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;
import com.db4o.test.*;


public class IndexQueryingIsFast {
    
    public int _int;
    
    public String _string;
    
    public IndexQueryingIsFast() {
        
    }
    
    public IndexQueryingIsFast(int i, String str) {
        _int = i;
        _string = str;
    }

    public void configure(){
        ObjectClass oc = Db4o.configure().objectClass(this.getClass());
        oc.objectField("_int").indexed(true);
        oc.objectField("_string").indexed(true);
    }
    
    public void store(){
        Test.deleteAllInstances(this);
        for (int i = 0; i < 5000; i++) {
            Test.store(new IndexQueryingIsFast(i, "" + i));
        }
    }
    
    public void _test(){
        
        Query q = Test.query();
        q.constrain(IndexQueryingIsFast.class);
        q.descend("_int").constrain(new Integer(3));
        check(q);
        
        q = Test.query();
        q.constrain(IndexQueryingIsFast.class);
        q.descend("_string").constrain("3");
        check(q);
        
        
    }

    private void check(Query q) {
        long start = System.currentTimeMillis();
        ObjectSet objectSet = q.execute();
        long stop = System.currentTimeMillis();
        Test.ensure(objectSet.size() == 1);
        IndexQueryingIsFast iqiF = (IndexQueryingIsFast) objectSet.next();
        Test.ensure(iqiF._int == 3);
        long duration = stop - start;
        long max = Test.isClientServer() ? 250 : 50;
        Test.ensure(duration < max);
        if(duration >= max){
            System.out.println("Indexed query too slow: " + duration + "ms");
        }
        
    }
    
    
    

}
