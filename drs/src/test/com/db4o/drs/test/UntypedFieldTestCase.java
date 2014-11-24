package com.db4o.drs.test;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class UntypedFieldTestCase extends DrsTestCase {
	
	public void testUntypedString() {
		assertUntypedReplication("42");
	}
	
	public void testUntypedStringArray() {
		assertUntypedReplication(new Object[] { "42" });
	}
	
	public void testUntypedStringJaggedArray() {
		assertJaggedArray("42");
	}
	
	public void testUntypedReferenceTypeJaggedArray() {
		assertJaggedArray(new UntypedFieldData(42));
	}
	
	public void testUntypedDate() {
		assertUntypedReplication(new Date(100, 2, 2));
	}
	
	public void testUntypedDateArray() {
		assertUntypedReplication(new Object[] { new Date(100, 2, 2) });
	}
	
	public void testUntypedMixedArray() {
		assertUntypedReplication(new Object[] { "42", new UntypedFieldData(42) });
		Assert.areEqual(42, ((UntypedFieldData)singleReplicatedInstance(UntypedFieldData.class)).getId());
	}
	
	public void testArrayAsCloneable() {
		Object[] array = new Object[] { "42", new UntypedFieldData(42) };
		ItemWithCloneable replicated = (ItemWithCloneable)replicate(new ItemWithCloneable(array));
		assertEquals(array, replicated.value);
	}

	private void assertUntypedReplication(Object data) {
		assertEquals(data, replicateItem(data).getUntyped());
	}
	
	private void assertJaggedArray(Object data) {
		Object[] expected = new Object[] { new Object[] { data } };
		Object[] actual = (Object[])replicateItem(expected).getUntyped();
		Assert.areEqual(expected.length, actual.length);
		
		Object[] nested = (Object[])actual[0];
		Object actualValue = nested[0];
		Assert.areEqual(data, actualValue);
		
		assertNotSame(data, actualValue);
	}

	private void assertNotSame(Object expectedReference, Object actual) {
		if (!isPrimitive(expectedReference.getClass())) {
			Assert.areNotSame(expectedReference, actual);
		}
	}

	private boolean isPrimitive(Class klass) {
		if (klass.isPrimitive()) return true;
		if (klass == String.class) return true;
		if (klass == Date.class) return true;
		return false;
	}
	
	private void assertEquals(Object expected, Object actual) {
		if (expected instanceof Object[]) {
			assertEquals((Object[])expected, (Object[])actual);
		} else {
			Assert.areEqual(expected, actual);
			assertNotSame(expected, actual);
		}
	}

	private void assertEquals(Object[] expectedArray, Object[] actualArray) {
		ArrayAssert.areEqual(expectedArray, actualArray);
		for (int i=0; i<expectedArray.length; ++i) {
			assertNotSame(expectedArray[i], actualArray[i]);
		}
	}

	private UntypedFieldItem replicateItem(Object data) {
		return (UntypedFieldItem) replicate(new UntypedFieldItem(data));
	}

	private Object replicate(Object item) {
		a().provider().storeNew(item);
		a().provider().commit();
		
		replicateAll(a().provider(), b().provider());
		
		return singleReplicatedInstance(item.getClass());
	}

	private Object singleReplicatedInstance(Class klass) {
		ObjectSet found = b().provider().getStoredObjects(klass);
		Assert.areEqual(1, found.size());
		return found.get(0);
	}

}
