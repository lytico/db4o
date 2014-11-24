/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent;

import java.util.*;

import com.db4o.config.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public abstract class ActivatableCollectionTestCaseBase<C extends Collection<CollectionElement>> extends AbstractDb4oTestCase {

	@Override
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentPersistenceSupport());
	}

	@Override
	protected void store() throws Exception {
		C collection = newActivatableCollection();
		CollectionHolder<C> item = new CollectionHolder<C>();
		item._collection = collection;
		store(item);
	}

	protected void assertAreEqual(C elements, C singleList) {
		IteratorAssert.sameContent(elements.iterator(), singleList.iterator());
	}

	protected CollectionHolder<C> singleHolder() {
		return retrieveOnlyInstance(CollectionHolder.class);
	}

	protected C singleCollection() {
		return singleHolder()._collection;
	}

	protected abstract C newActivatableCollection();
}