/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.staging;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ObjectVersionTest extends AbstractDb4oTestCase {
	
	protected void configure(Configuration config) {
		config.generateUUIDs(ConfigScope.GLOBALLY);
		config.generateCommitTimestamps(true);
	}

	public void test() {
		final ExtObjectContainer oc = this.db();
		Item object = new Item("c1");
		
		oc.store(object);
		
		ObjectInfo objectInfo1 = oc.getObjectInfo(object);
		long oldVer = objectInfo1.getCommitTimestamp();

		//Update
		object.setName("c3");
		oc.store(object);

		ObjectInfo objectInfo2 = oc.getObjectInfo(object);
		long newVer = objectInfo2.getCommitTimestamp();

		Assert.isNotNull(objectInfo1.getUUID());
		Assert.isNotNull(objectInfo2.getUUID());

		Assert.isTrue(oldVer > 0);
		Assert.isTrue(newVer > 0);

		Assert.areEqual(objectInfo1.getUUID(), objectInfo2.getUUID());
		Assert.isTrue(newVer > oldVer);
	}
	
    public static class Item{
    	
        public String name;
        
        public Item() {
        }
        
        public Item(String name_) {
            this.name = name_;
        }

        public String getName() {
            return name;
        }
        
        public void setName(String name_){
        	name = name_;
        }

    }

}
