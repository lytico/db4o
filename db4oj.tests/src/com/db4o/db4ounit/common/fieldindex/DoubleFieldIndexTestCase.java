/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class DoubleFieldIndexTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new DoubleFieldIndexTestCase().runSolo();
	}
	
	public static class Item {        
        public double value;
        
        public Item() {            
        }

        public Item(double value_) {
            value = value_;
        }
	}
    
    protected void configure(Configuration config){
        indexField(config,Item.class, "value");
    }
    
    protected void store() throws Exception {
    	db().store(new Item(0.5));
    	db().store(new Item(1.1));
    	db().store(new Item(2));
    }
    
    public void testEqual() {
    	final Query query = newQuery(Item.class);
		query.descend("value").constrain(new Double(1.1));
    	
    	assertItems(new double[] { 1.1 }, query.execute());
    }
    
    public void testGreater() {
    	final Query query = newQuery(Item.class);
    	final Query descend = query.descend("value");
		descend.constrain(new Double(1)).greater();
		descend.orderAscending();
    	
    	assertItems(new double[] { 1.1, 2 }, query.execute());
    }

	private void assertItems(double[] expected, ObjectSet set) {
		ArrayAssert.areEqual(expected, toDoubleArray(set));
	}

	private double[] toDoubleArray(ObjectSet set) {
		double[] array = new double[set.size()];
		for (int i = 0; i < array.length; i++) {
			array[i] = ((Item)set.next()).value;
		}
		return array;
	}

}
