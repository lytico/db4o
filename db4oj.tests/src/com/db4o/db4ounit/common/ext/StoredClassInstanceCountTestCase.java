package com.db4o.db4ounit.common.ext;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class StoredClassInstanceCountTestCase extends AbstractDb4oTestCase {

	public static class ItemA {
	}

	public static class ItemB {
	}

	private static final int COUNT_A = 5;
	
	protected void store() throws Exception {
		for(int idx = 0; idx < COUNT_A; idx++) {
			store(new ItemA());
		}
		store(new ItemB());
	}

	public void testInstanceCount() {
		assertInstanceCount(ItemA.class, COUNT_A);
		assertInstanceCount(ItemB.class, 1);
		store(new ItemA());
		deleteAll(ItemB.class);
		assertInstanceCount(ItemA.class, COUNT_A + 1);
		assertInstanceCount(ItemB.class, 0);
	}

	public void testTransactionalInstanceCount() {
		if(!isMultiSession()) {
			return;
		}
		ExtObjectContainer otherClient = openNewSession();
		store(new ItemA());
		deleteAll(ItemB.class);
		assertInstanceCount(db(), ItemA.class, COUNT_A + 1);
		assertInstanceCount(db(), ItemB.class, 0);
		assertInstanceCount(otherClient, ItemA.class, COUNT_A);
		assertInstanceCount(otherClient, ItemB.class, 1);
		db().commit();
		assertInstanceCount(db(), ItemA.class, COUNT_A + 1);
		assertInstanceCount(db(), ItemB.class, 0);
		assertInstanceCount(otherClient, ItemA.class, COUNT_A + 1);
		assertInstanceCount(otherClient, ItemB.class, 0);
		otherClient.commit();
		otherClient.store(new ItemB());
		assertInstanceCount(db(), ItemB.class, 0);
		assertInstanceCount(otherClient, ItemB.class, 1);
		otherClient.commit();
		assertInstanceCount(db(), ItemB.class, 1);
		assertInstanceCount(otherClient, ItemB.class, 1);
		otherClient.close();
	}
	
	private void assertInstanceCount(Class<?> clazz, int expectedCount) {
		assertInstanceCount(db(), clazz, expectedCount);
	}

	private void assertInstanceCount(ExtObjectContainer container, Class<?> clazz, int expectedCount) {
		StoredClass storedClazz = container.ext().storedClass(clazz);
		Assert.areEqual(expectedCount, storedClazz.instanceCount());
	}
	
	public static void main(String[] args) {
		new StoredClassInstanceCountTestCase().runAll();
	}
}
