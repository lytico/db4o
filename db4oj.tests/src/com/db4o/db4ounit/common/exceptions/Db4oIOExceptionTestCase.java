/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;

public class Db4oIOExceptionTestCase extends Db4oIOExceptionTestCaseBase {

	public static void main(String[] args) {
		new Db4oIOExceptionTestCase().runSolo();
	}

	protected void configure(Configuration config) {
		super.configure(config);
	}

	public void testActivate() throws Exception {
		store(new Item(3));
		fixture().config().activationDepth(1);
		fixture().reopen(this);
		final Item item = (Item) retrieveOnlyInstance(Item.class);
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().activate(item, 3);
			}
		});
	}
	
	public void testClose() {
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().close();
			}
		});
	}
	
	public void testCommit() {
		store(new Item(0));
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().commit();
			}
		});
	}
	
	public void testDelete() throws Exception {
		store(new Item(3));
		final Item item = (Item) retrieveOnlyInstance(Item.class);
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().delete(item);
			}
		});
	}
	
	public void testGet() throws Exception {
		store(new Item(3));
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().queryByExample(Item.class);
			}
		});
	}
	
	public void testGetAll() throws Exception {
		store(new Item(3));
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				ObjectSet os = db().queryByExample(null);
				while(os.hasNext()) {
					os.next();
				}
			}
		});
	}
	
	public void testQuery() throws Exception {
		store(new Item(3));
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().query(Item.class);
			}
		});
	}
	
	public void testRollback() throws Exception {
		store(new Item(3));
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().rollback();
			}
		});
	}
	
	public void testSet() throws Exception {
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().store(new Item(3));
			}
		});
	}

	public void testGetByUUID() throws Exception {
		fixture().config().generateUUIDs(ConfigScope.GLOBALLY);
		fixture().reopen(this);
		Item item = new Item(1);
		store(item);
		final Db4oUUID uuid = db().getObjectInfo(item).getUUID();
		fixture().reopen(this);
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().getByUUID(uuid);
			}
		});
	}

	public static class Item {
		public Item(int depth) {
			member = new DeepMemeber(depth);
		}

		public DeepMemeber member;
	}

	public static class DeepMemeber {
		public DeepMemeber(int depth) {
			if (depth > 0) {
				member = new DeepMemeber(--depth);
			}
		}

		public DeepMemeber member;
	}

}
