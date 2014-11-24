package com.db4o.drs.test;

import static com.db4o.drs.test.data.SimpleEnum.*;

import com.db4o.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.test.data.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
public class Db4oEnumTestCase extends DrsTestCase {

	public void testBasicQuery() {

		storeNewTo(a(), new SimpleEnumContainer(ONE));

		replicateAll(a().provider(), b().provider());

		ExtObjectContainer db = ((Db4oReplicationProvider) b().provider()).getObjectContainer();

		Query q = db.query();
		q.constrain(SimpleEnumContainer.class);
		q.descend("value").descend("value").constrain(1);
		ObjectSet<Object> list = q.execute();
		Assert.areEqual(1, list.size());
	}

	public void testQueryWithMutableEnumContainer() {

		storeNewTo(a(), new SimpleEnumContainer(ONE));

		replicateAll(a().provider(), b().provider());

		SimpleEnumContainer containerInB = (SimpleEnumContainer) getOneInstance(b(), SimpleEnumContainer.class);

		containerInB.setValue(TWO);
		updateTo(b(), containerInB);

		replicateAll(b().provider(), a().provider());

		ExtObjectContainer db = ((Db4oReplicationProvider) a().provider()).getObjectContainer();

		Query q = db.query();
		q.constrain(SimpleEnumContainer.class);
		q.descend("value").descend("value").constrain(2);
		ObjectSet<Object> list = q.execute();
		Assert.areEqual(1, list.size());

	}
}
