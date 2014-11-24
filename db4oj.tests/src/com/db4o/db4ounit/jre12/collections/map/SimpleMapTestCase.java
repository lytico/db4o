package com.db4o.db4ounit.jre12.collections.map;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class SimpleMapTestCase extends AbstractDb4oTestCase{
	
	protected void configure(Configuration config) {
		config.generateUUIDs(ConfigScope.GLOBALLY);
	}

	public static void main(String[] args) {
        new SimpleMapTestCase().runNetworking();
    }
	
	public void testGetByUUID() {
		MapContent c1 = new MapContent("c1");
		db().store(c1);	//comment me bypass the bug

		//db().getObjectInfo(c1).getUUID();	//Uncomment me bypass the bug

		MapHolder mh = new MapHolder("h1");
		mh.map.put("key1", c1);

		db().store(mh);	//comment me bypass the bug

		Db4oUUID uuid = db().getObjectInfo(c1).getUUID();

		Assert.isNotNull(db().getByUUID(uuid));	//This line fails when Test.clientServer = true;
	}
}
