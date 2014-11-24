/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.uuid;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeleteUUIDTestCase extends AbstractDb4oTestCase {

	private Db4oUUID _uuid;
	
	public static class Item {
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.generateUUIDs(ConfigScope.GLOBALLY);
	}

	@Override
	protected void store() throws Exception {
		Item item = new Item();
		store(item);
		_uuid = db().getObjectInfo(item).getUUID();
	}
	
	public void testDelete() throws Exception {
		Item item = retrieveOnlyInstance(Item.class);
		db().delete(item);
		Assert.isNull(db().getByUUID(_uuid));
	}

	public void testDeleteCommit() throws Exception {
		Item item = retrieveOnlyInstance(Item.class);
		db().delete(item);
		db().commit();
		Assert.isNull(db().getByUUID(_uuid));
	}

	public void testDeleteRollback() throws Exception {
		Item item = retrieveOnlyInstance(Item.class);
		db().delete(item);
		db().rollback();
		Assert.isNotNull(db().getByUUID(_uuid));
	}

}
