/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test;

import java.util.*;

import com.db4o.config.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.fixtures.*;


public class SingleTypeCollectionReplicationTest extends FixtureBasedTestSuite {
	
	private static final FixtureVariable TRANSPARENT_ACTIVATION_FIXTURE = FixtureVariable.newInstance("Transparent Activation");

	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
			new SubjectFixtureProvider(new CollectionHolderFactory[] {
				collection1(),
				collection2(),
				collection3(),
			}),
			new SimpleFixtureProvider(TRANSPARENT_ACTIVATION_FIXTURE, (Object[])LabeledObject.forObjects(new Object[]{false, true})),
		};
	}
	
	public static abstract class CollectionHolderFactory implements Labeled {
		public abstract CollectionHolder newCollectionHolder();
	}

	private CollectionHolderFactory collection1() {
		return new CollectionHolderFactory(){
			@Override
			public CollectionHolder newCollectionHolder() {
				return initialize(
						new CollectionHolder(
								"Hashtable",
								new Hashtable(),
								new HashSet(),
								new LinkedList()));

			}
			public String label() {
				return "Hashtable";
			}
		};
	}
	
	private CollectionHolderFactory collection2() {
		return new CollectionHolderFactory(){
			@Override
			public CollectionHolder newCollectionHolder() {
				return initialize(
						new CollectionHolder(
								"HashMap",
								new HashMap<String, String>(),
								new HashSet(),
								new ArrayList<String>()));

			}
			public String label() {
				return "HashMap";
			}
		};
	}
	
	private CollectionHolderFactory collection3() {
		return new CollectionHolderFactory(){
			@Override
			public CollectionHolder newCollectionHolder() {
				return initialize(
						new CollectionHolder(
								"TreeMap",
								new TreeMap<String, String>(),
								new HashSet(),
								new ArrayList()));

			}
			public String label() {
				return "TreeMap";
			}
		};
	}

	private CollectionHolder initialize(CollectionHolder h1) {
		h1.map().put("1", "one");
		h1.map().put("2", "two");
		h1.set().add("two");
		h1.list().add("three");
		return h1;
	}

	@Override
	public Class[] testUnits() {
		return new Class[] { TestUnit.class };
	}
	
	public static class TestUnit extends DrsTestCase {
		
		@Override
		protected void configure(Configuration config) {
			LabeledObject transparentActivation = (LabeledObject) TRANSPARENT_ACTIVATION_FIXTURE.value();
			if((Boolean)transparentActivation.value()){
				config.add(new TransparentActivationSupport());
			}
		}
	
		public void test() {
			
			CollectionHolder h1 = subject();
			
			storeNewAndCommit(a().provider(), h1);
			
			replicateAll(a().provider(), b().provider());
			
			Iterator it = b().provider().getStoredObjects(CollectionHolder.class).iterator();
			Assert.isTrue(it.hasNext());
			
			CollectionHolder replica = (CollectionHolder) it.next();
			
			b().provider().activate(replica);
			assertSameClassIfDb4o(h1.map(), replica.map());
			for (Object key : h1.map().keySet()) {
				b().provider().activate(replica.map());
				Assert.areEqual(h1.map().get(key), replica.map().get(key));
			}
			
			assertSameClassIfDb4o(h1.set(), replica.set());
			for (Object element : h1.set()) {
				Assert.isTrue(replica.set().contains(element));
			}
			
			assertSameClassIfDb4o(h1.list(), replica.list());
			Assert.areEqual(h1.list().size(), replica.list().size());
			CollectionAssert.areEqual(h1.list(), replica.list());
						
		}

		private CollectionHolder subject() {
			CollectionHolderFactory factory = (CollectionHolderFactory)SubjectFixtureProvider.value();
			return factory.newCollectionHolder();
		}

		private void assertSameClassIfDb4o(final Object expectedInstance,
				final Object actualInstance) {
			if (!isDb4oProvider(a())) return;
			if (!isDb4oProvider(b())) return;
			
			Assert.areSame(expectedInstance.getClass(), actualInstance.getClass());
		}
	
		private boolean isDb4oProvider(final DrsProviderFixture fixture) {
			return fixture.provider() instanceof Db4oReplicationProvider;
		}
		
		private void storeNewAndCommit(
				final TestableReplicationProviderInside provider,
				CollectionHolder h1) {
			provider.storeNew(h1);
			provider.activate(h1);
			provider.commit();
		}
	}
}
