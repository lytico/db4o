/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;
import com.db4o.test.*;


public class MultiDelete {
    
    public MultiDelete child;
    public String name;
    public Object forLong;
    public Long myLong;
    public Object[] untypedArr;
    public Long[] typedArr;
    
    
    public void configure(){
        Db4o.configure().objectClass(this).cascadeOnDelete(true);
        Db4o.configure().objectClass(this).cascadeOnUpdate(true);
    }
    
    public void store(){
        MultiDelete md = new MultiDelete();
        md.name = "killmefirst";
        md.setMembers();
        md.child = new MultiDelete();
        md.child.setMembers();
        Test.store(md);
    }
    
    private void setMembers(){
        forLong = new Long(100);
        myLong = new Long(100);
        untypedArr = new Object[]{
            new Long(10),
            "hi",
            new MultiDelete()
        };
        typedArr = new Long[]{
            new Long(3),
            new Long(7),
            new Long(9),
        };
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(MultiDelete.class);
        q.descend("name").constrain("killmefirst");
        ObjectSet objectSet = q.execute();
        Test.ensureEquals(1,objectSet.size());
        MultiDelete md = (MultiDelete)objectSet.next();
        ExtObjectContainer oc = Test.objectContainer();
        long id = oc.getID(md);
        oc.delete(md);
        
        MultiDelete afterDelete = (MultiDelete)oc.getByID(id);
        oc.delete(md);
    }
    

}
