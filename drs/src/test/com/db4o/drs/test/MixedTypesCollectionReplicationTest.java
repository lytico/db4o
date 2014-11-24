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

import com.db4o.ObjectSet;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.inside.GenericReplicationSession;
import com.db4o.drs.test.data.*;

import db4ounit.Assert;

public class MixedTypesCollectionReplicationTest extends DrsTestCase {

	protected void actualTest() {
		if (!a().provider().supportsHybridCollection()) return;
		if (!b().provider().supportsHybridCollection()) return;

		CollectionHolder h1 = new CollectionHolder("h1");
		CollectionHolder h2 = new CollectionHolder("h2");

		h1.map().put("key", "value");
		h1.map().put("key2", h1);
		h1.map().put(h1, "value2");

		h2.map().put("key", h1);
		h2.map().put(h2, h1);

		h1.list().add("one");
		h1.list().add(h1);

		h2.list().add("two");
		h2.list().add(h1);
		h2.list().add(h2);

		h1.set().add("one");
		h1.set().add(h1);

		h2.set().add("two");
		h2.set().add(h1);
		h2.set().add(h2);


		b().provider().storeNew(h2);
		b().provider().storeNew(h1);

		final ReplicationSession replication = new GenericReplicationSession(a().provider(), b().provider());

		replication.replicate(h2); //Traverses to h1.

		replication.commit();

		Iterator objects = a().provider().getStoredObjects(CollectionHolder.class).iterator();
		check(nextCollectionHolder(objects), h1, h2);
		check(nextCollectionHolder(objects), h1, h2);
	}

	private CollectionHolder nextCollectionHolder(Iterator objects) {
		Assert.isTrue(objects.hasNext());
		return (CollectionHolder) objects.next();
	}

	private void check(CollectionHolder holder, CollectionHolder original1, CollectionHolder original2) {
		Assert.isTrue(holder != original1);
		Assert.isTrue(holder != original2);

		if (holder.name().equals("h1"))
			checkH1(holder);
		else
			checkH2(holder);
	}

	private void checkH1(CollectionHolder holder) {
		Assert.areEqual("value", holder.map().get("key"));
		Assert.areEqual(holder, holder.map().get("key2"));
		Assert.areEqual("value2", holder.map().get(holder));

		Assert.areEqual("one", holder.list().get(0));
		Assert.areEqual(holder, holder.list().get(1));

		Assert.isTrue(holder.set().contains("one"));
		Assert.isTrue(holder.set().contains(holder));
	}

	private void checkH2(CollectionHolder holder) {
		Assert.areEqual("h1", ((CollectionHolder) holder.map().get("key")).name());
		Assert.areEqual("h1", ((CollectionHolder) holder.map().get(holder)).name());

		Assert.areEqual("two", holder.list().get(0));
		Assert.areEqual("h1", ((CollectionHolder) holder.list().get(1)).name());
		Assert.areEqual(holder, holder.list().get(2));

		Assert.isTrue(holder.set().remove("two"));
		Assert.isTrue(holder.set().remove(holder));
		CollectionHolder remaining = nextCollectionHolder(holder.set().iterator());
		Assert.areEqual("h1", remaining.name());
	}

	public void test() {
		actualTest();
	}

}
