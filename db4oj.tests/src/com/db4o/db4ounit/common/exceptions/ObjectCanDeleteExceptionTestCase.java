/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ObjectCanDeleteExceptionTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {

	public static void main(String[] args) {
		new ObjectCanDeleteExceptionTestCase().runSolo();
	}

	public static class Item {
		public boolean objectCanDelete(ObjectContainer container) {
			throw new ItemException();
		}
	}

	public void test() {
		final Item item = new Item();
		store(item);
		Assert.expect(ReflectException.class, ItemException.class,
				new CodeBlock() {
					public void run() throws Throwable {
						db().delete(item);
						db().commit();
					}
				});
	}
}
