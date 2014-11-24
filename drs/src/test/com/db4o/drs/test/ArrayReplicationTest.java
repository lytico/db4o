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

import com.db4o.drs.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class ArrayReplicationTest extends DrsTestCase {

	public void test() {
	
		if (!a().provider().supportsMultiDimensionalArrays()) return;
		if (!b().provider().supportsMultiDimensionalArrays()) return;

		ArrayHolder h1 = new ArrayHolder("h1");
		ArrayHolder h2 = new ArrayHolder("h2");

		h1._array = new ArrayHolder[]{h1};
		h2._array = new ArrayHolder[]{h1, h2, null};

		h1._arrayN = new ArrayHolder[][]{{h1}};

		h2._arrayN = new ArrayHolder[][]{{h1, null}, {null, h2}, {null, null}}; //TODO Fix ReflectArray.shape() and test with innermost arrays of varying sizes:  {{h1}, {null, h2}, {null}}

		b().provider().storeNew(h2);
		b().provider().storeNew(h1);
		b().provider().commit();

		final ReplicationSession replication = new GenericReplicationSession(a().provider(), b().provider(), null, _fixtures.reflector);

		replication.replicate(h2); //Traverses to h1.

		replication.commit();

		Iterator objects = a().provider().getStoredObjects(ArrayHolder.class).iterator();
		checkNext(objects);
		checkNext(objects);
		Assert.isFalse(objects.hasNext());
	}

	private void checkNext(Iterator objects) {
		Assert.isTrue(objects.hasNext());
		check((ArrayHolder) objects.next());
	}
	
	private void check(ArrayHolder holder) {
		if (holder.getName().equals("h1"))
			checkH1(holder);
		else
			checkH2(holder);
	}

	protected void checkH1(ArrayHolder holder) {
		Assert.areEqual(holder.array()[0], holder);
		Assert.areEqual(holder.arrayN()[0][0], holder);
	}

	protected void checkH2(ArrayHolder holder) {
		Assert.areEqual(holder.array()[0].getName(), "h1");
		Assert.areEqual(holder.array()[1], holder);
		Assert.areEqual(holder.array()[2], null);

		Assert.areEqual(holder.arrayN()[0][0].getName(), "h1");
		Assert.areEqual(holder.arrayN()[1][0], null);
		Assert.areEqual(holder.arrayN()[1][1], holder);
		Assert.areEqual(holder.arrayN()[2][0], null);
	}

}
