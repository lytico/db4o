/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.fatalerror;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class NativeQueryTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public String str;

		public Item(String s) {
			str = s;
		}
	}

	public static void main(String[] args) {
		new NativeQueryTestCase().runSoloAndClientServer();
	}

	protected void store() throws Exception {
		store(new Item("hello"));
	}

	public void _test() {
		Assert.expect(NQError.class, new CodeBlock() {
			public void run() throws Throwable {
				Predicate fatalErrorPredicate = new FatalErrorPredicate();
				db().query(fatalErrorPredicate);
			}
		});
		Assert.isTrue(db().isClosed());
	}

	public static class FatalErrorPredicate extends Predicate {
		public boolean match(Object item) {
			throw new NQError("nq error!");
		}
	}
	
	public static class NQError extends Error {
		public NQError(String msg) {
			super(msg);
		}
	}
}
