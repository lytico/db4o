package com.db4o.drs.test;

import java.util.*;

import com.db4o.drs.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;
import db4ounit.fixtures.*;

public class TheSimplestTestSuite extends FixtureBasedTestSuite {

	public static class TheSimplest extends DrsTestCase {

		public void test() {
			storeInA();
			replicate();
			modifyInB();
			replicate2();
			modifyInA();
			replicate3();
		}

		private void replicate3() {
			replicateClass(a().provider(), b().provider(), SPCChild.class);

			ensureNames(a(), "c3");
			ensureNames(b(), "c3");
		}

		private void modifyInA() {
			SPCChild child = getTheObject(a());

			child.setName("c3");

			a().provider().update(child);
			a().provider().commit();

			ensureNames(a(), "c3");
		}

		private void replicate2() {
			replicateAll(b().provider(), a().provider());

			ensureNames(a(), "c2");
			ensureNames(b(), "c2");
		}

		private void storeInA() {
			SPCChild child = new SPCChild("c1");
			
			a().provider().storeNew(child);
			a().provider().commit();
			
			ensureNames(a(), "c1");
		}
			
		private void replicate() {
			replicateAll(a().provider(), b().provider());

			ensureNames(a(), "c1");
			ensureNames(b(), "c1");
		}
		
		private void modifyInB() {
			SPCChild child = getTheObject(b());

			child.setName("c2");
			b().provider().update(child);
			b().provider().commit();

			ensureNames(b(), "c2");
		}
		
		private void ensureNames(DrsProviderFixture fixture, String childName) {
			ensureOneInstance(fixture, SPCChild.class);
			SPCChild child = getTheObject(fixture);
			Assert.areEqual(childName,child.getName());
		}


		private SPCChild getTheObject(DrsProviderFixture fixture) {
			return (SPCChild) getOneInstance(fixture, SPCChild.class);
		}

		protected void replicateClass(TestableReplicationProviderInside providerA, TestableReplicationProviderInside providerB, Class clazz) {
			//System.out.println("ReplicationTestcase.replicateClass");
			ReplicationSession replication = Replication.begin(providerA, providerB, null, _fixtures.reflector);
			Iterator allObjects = providerA.objectsChangedSinceLastReplication(clazz).iterator();
			while (allObjects.hasNext()) {
				final Object obj = allObjects.next();
				//System.out.println("obj = " + obj);
				replication.replicate(obj);
			}
			replication.commit();
		}

	}

	private static final FixtureVariable CONSTRUCTOR_CONFIG_FIXTURE = new FixtureVariable("config");

	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
			new SimpleFixtureProvider(TheSimplestTestSuite.CONSTRUCTOR_CONFIG_FIXTURE,
				new Object[] {
					Boolean.FALSE,
					Boolean.TRUE,
				}
			)	
		};
	}

	@Override
	public Class[] testUnits() {
		return new Class[] {
				TheSimplest.class,
		};
	}
}
