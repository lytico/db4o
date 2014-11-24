/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.conjunctions;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;


public class CJChildField implements CJHasID{
    
    
    public CJChild _child;
    
    public int _id;
    
    public CJChildField(){
    }
    
    public CJChildField(int id){
        _id = id;
        _child = new CJChild(_id);
    }
    
    public void store(){
        Test.deleteAllInstances(CJChildField.class);
        for (int i = 0; i < 20; i++) {
            store(i);
        }
    }
    
    public void configure(){
        Db4o.configure().objectClass(this).objectField("_child").indexed(true);
        Db4o.configure().objectClass(CJChild.class).objectField("_id").indexed(true);
    }
    
    private void store(int i){
        Test.store(new CJChildField(i));
    }
    
    
    public void test(){
        
        Query q = Test.query();
        q.constrain(this.getClass());
        Query qId = q.descend("_child").descend("_id");
        qId.constrain(new Integer(1)).greater();
        qId.constrain(new Integer(2)).smaller().equal();
        ConjunctionsTestSuite.expect(q, new int[]{2});
        

        q = Test.query();
        q.constrain(this.getClass());
        qId = q.descend("_child").descend("_id");
        qId.constrain(new Integer(0)).greater();
        qId.constrain(new Integer(1)).greater();
        qId.constrain(new Integer(2)).smaller().equal();
        qId.constrain(new Integer(3)).smaller().equal();
        qId.constrain(new Integer(4)).smaller().equal();
        ConjunctionsTestSuite.expect(q, new int[]{2});
        
        store(1);

        q = Test.query();
        q.constrain(this.getClass());
        qId = q.descend("_child").descend("_id");
        qId.constrain(new Integer(1)).smaller().equal();
        ConjunctionsTestSuite.expect(q, new int[]{0,1,1});
        

    }


    public int getID() {
        return _id;
    }
    

}
