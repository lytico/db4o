/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.set;

import java.util.*;

import com.db4o.db4ounit.jre12.collections.transparent.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.fixtures.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableSetAPITestSuite extends FixtureBasedTestSuite implements Db4oTestCase {

	private static FixtureVariable<CollectionSpec<Set<CollectionElement>>> SET_SPEC =
		new FixtureVariable<CollectionSpec<Set<CollectionElement>>>("set");

	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
				new Db4oFixtureProvider(),
				new SimpleFixtureProvider(SET_SPEC,
						new CollectionSpec<HashSet<CollectionElement>>(
								HashSet.class, 
								CollectionFactories.activatableHashSetFactory(),
								CollectionFactories.plainHashSetFactory())  ,
						new CollectionSpec<TreeSet<CollectionElement>>(
										TreeSet.class, 
										CollectionFactories.activatableTreeSetFactory(),
										CollectionFactories.plainTreeSetFactory())
				),
		};
	}

	@Override
	public Class[] testUnits() {
		return new Class[] {
				ActivatableSetAPITestUnit.class
		};
	}
	
	public static class ActivatableSetAPITestUnit extends ActivatableCollectionAPITestUnit<Set<CollectionElement>> {
		@Override
		protected CollectionSpec<Set<CollectionElement>> currentCollectionSpec() {
			return SET_SPEC.value();
		}
	}
}
