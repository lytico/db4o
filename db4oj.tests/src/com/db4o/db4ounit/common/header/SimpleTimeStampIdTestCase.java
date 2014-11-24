/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.header;

import com.db4o.config.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class SimpleTimeStampIdTestCase extends AbstractDb4oTestCase implements
		OptOutMultiSession {

	public static void main(String[] arguments) {
		new SimpleTimeStampIdTestCase().runSolo();
	}

	public static class STSItem {

		public String _name;

		public STSItem() {
		}

		public STSItem(String name) {
			_name = name;
		}
	}

	protected void configure(Configuration config) {
		ObjectClass objectClass = config.objectClass(STSItem.class);
		objectClass.generateUUIDs(true);
		config.generateCommitTimestamps(true);
	}

	protected void store() {
		db().store(new STSItem("one"));
	}

	public void test() throws Exception {
		STSItem item = (STSItem) db().queryByExample(STSItem.class).next();

		long version = db().getObjectInfo(item).getCommitTimestamp();
		Assert.isGreater(0, version);
		Assert.isGreaterOrEqual(version, currentVersion());

		reopen();

		STSItem item2 = new STSItem("two");
		db().store(item2);
		db().commit();

		long secondVersion = db().getObjectInfo(item2).getCommitTimestamp();

		Assert.isGreater(version, secondVersion);
		Assert.isGreaterOrEqual(secondVersion, currentVersion());
	}

	private long currentVersion() {
		return ((LocalObjectContainer) db()).currentVersion();
	}
}
