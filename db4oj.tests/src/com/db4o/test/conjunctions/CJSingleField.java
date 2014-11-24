/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.conjunctions;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;

public class CJSingleField implements CJHasID{
    
    public int _id;
    
    public CJSingleField(){
    }
    
    public CJSingleField(int id){
        _id = id;
    }
    
    public void configure(){
        Db4o.configure().objectClass(this).objectField("_id").indexed(true);
    }
    
    public void store(){
        Test.deleteAllInstances(CJSingleField.class);
        store(1);
        store(2);
        store(3);
        store(3);
    }
    
    private void store(int i){
        Test.store(new CJSingleField(i));
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(this.getClass());
        Query qId = q.descend("_id");
        qId.constrain(new Integer(1)).greater();
        qId.constrain(new Integer(2)).smaller().equal();
        ConjunctionsTestSuite.expect(q, new int[]{2});
    }
    
    public int getID() {
        return _id;
    }

}
