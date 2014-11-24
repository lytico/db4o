/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.drs.test;

import java.util.*;

import com.db4o.drs.test.data.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.array.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

import db4ounit.*;
import db4ounit.fixtures.*;


public class ArrayTestSuite extends FixtureBasedTestSuite {
	
	public static class TestUnit extends DrsTestCase {
		
		public void test() {
			ItemWithUntypedField item = new ItemWithUntypedField(subject());
			storeToProviderA(item);
			
			replicatedAllToB();
			
			ItemWithUntypedField replicated = replicatedItem();
			Assert.isNotNull(replicated.array());
			
			Iterator4Assert.areEqual(arrayIterator(item.array()), arrayIterator(replicated.array()));
		}

		private Iterator4 arrayIterator(Object array) {
			return ArrayHandler.iterator(reflectClass(array), array);
		}

		private ReflectClass reflectClass(Object array) {
			return genericReflector().forObject(array);
		}

		private GenericReflector genericReflector() {
			return new GenericReflector(null, Platform4.reflectorForType(getClass()));
		}

		private void replicatedAllToB() {
			replicateAll(a().provider(), b().provider());
		}

		private void storeToProviderA(ItemWithUntypedField item) {
			a().provider().storeNew(item);
			a().provider().commit();
		}

		private ItemWithUntypedField replicatedItem() {
			final Iterator iterator = b().provider().getStoredObjects(ItemWithUntypedField.class).iterator();
			if (iterator.hasNext()) {
				return (ItemWithUntypedField) iterator.next();
			}
			return null;
		}

		private Object subject() {
			return SubjectFixtureProvider.value();
		}
		
	}

	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
			new SubjectFixtureProvider(
					new Object[] {
						new Object[] { },
						new String[] { "foo", "bar" },
						new int[] { 42, -1, 0 },
						new int[][] { },
						new java.util.Date[] { new java.util.Date() },
					}),			
		};
	}

	@Override
	public Class[] testUnits() {
		return new Class[] {
			TestUnit.class,
		};
	}

}
