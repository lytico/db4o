package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TreeSetCustomComparableTestCase extends AbstractDb4oTestCase {
    
    
    public static class Item implements Comparable {
        
        public Item() {
            this.path = new TreeSet();
        }
        
        public Set path;
        
        public int compareTo(Object that) {
            return hashCode()-that.hashCode();
        }
        
    }
    
    public void store(){
        Map map=new TreeMap();
        map.put(new Item(),new TreeSet());
        store(map);
    }
    
    public void testRetrieveOnlyInstance(){
        TreeMap tm = (TreeMap) retrieveOnlyInstance(TreeMap.class);
        assertMap(tm);
    }

    public void testQueryByExample() {
    	TreeMap map = new TreeMap();
        ObjectSet result = db().queryByExample(map);
        Assert.areEqual(1, result.size());
        TreeMap tm = (TreeMap) result.next();
        assertMap(tm);
    }
    
    private void assertMap(TreeMap tm) {
	Assert.areEqual(1, tm.size());
        Iterator i = tm.keySet().iterator();
        Assert.isTrue(i.hasNext());
        Item item  = (Item)i.next();
        TreeSet ts = (TreeSet)tm.get(item);
        Assert.isNotNull(ts);
    }

}
