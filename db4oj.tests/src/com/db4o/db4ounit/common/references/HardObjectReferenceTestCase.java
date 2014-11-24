/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.references;

import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class HardObjectReferenceTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new HardObjectReferenceTestCase().runSolo();
    }
    
    public static class Item {
        
        public String _name;
        
        public Item(String name){
            _name = name;
        }

        public boolean equals(Object obj) {
            if (obj == null) return false;
            if (getClass() != obj.getClass()) return false;
            return _name.equals(((Item) obj)._name);
        }
    }
    
    public void testPeekPersisted(){
        Item item = new Item("one");
        store(item);
        int id = (int) db().getID(item);
        Assert.areEqual(item, peek(id)._object);
        db().delete(item);
        db().commit();
        Assert.isNull(peek(id));
    }

    private HardObjectReference peek(int id) {
        return HardObjectReference.peekPersisted(trans(),id, 1);
    }

}
