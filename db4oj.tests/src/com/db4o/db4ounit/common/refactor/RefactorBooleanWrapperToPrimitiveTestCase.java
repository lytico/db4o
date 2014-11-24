/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.refactor;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @sharpen.remove
 */
public class RefactorBooleanWrapperToPrimitiveTestCase extends AbstractDb4oTestCase {

	private static final int ID_VALUE = 42;

	public static class ItemBefore {
		public int _id;
		public Boolean _flag;
		
		public ItemBefore(int id, Boolean flag) {
			_id = id;
			_flag = flag;
		}
	}

	public static class ItemAfter {
		public int _id;
		public boolean _flag;
	}

	public void testDefaultNullWrapperValue() throws Exception {
		assertRefactor(ID_VALUE, null);
	}

	public void testFalseWrapperValue() throws Exception {
		assertRefactor(ID_VALUE, Boolean.FALSE);
	}

	public void testTrueWrapperValue() throws Exception {
		assertRefactor(ID_VALUE, Boolean.TRUE);
	}

	private void assertRefactor(int id, Boolean storedValue) throws Exception {
		store(new ItemBefore(id, storedValue));
		reopen();
		fileSession().storedClass(ItemBefore.class).rename(ItemAfter.class.getName());
		reopen();
		assertRetrieveAsPrimitive(id, storedValue);
		defragment();
		assertRetrieveAsPrimitive(id, storedValue);
	}

	private void assertRetrieveAsPrimitive(int id, Boolean flag) {
		ItemAfter item = retrieveOnlyInstance(ItemAfter.class);
		Assert.areEqual(id, item._id);
		Assert.areEqual(coerce(flag), item._flag);
	}
	
	private boolean coerce(Boolean wrapper) {
		return wrapper == null ? false : wrapper.booleanValue();
	}

}
