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
package com.db4o.drs.test.dotnet;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class StructTestCase extends DrsTestCase {
	
	Container template = new Container(new Value(42));
	
	public void test() {
        storeToProviderA();
        replicateAllToProviderB();
    }

    void storeToProviderA() {
        TestableReplicationProviderInside provider = a().provider();
        provider.storeNew(template);
        provider.commit();
        ensureContent(template, provider);
    }

    void replicateAllToProviderB() {
        replicateAll(a().provider(), b().provider());
        ensureContent(template, b().provider());
    }

    private void ensureContent(Container container,
            TestableReplicationProviderInside provider) {
        ObjectSet result = provider.getStoredObjects(container.getClass());
        Assert.areEqual(1, result.size());

        Container c = next(result);
        Assert.areEqual(template.getValue(), c.getValue());
    }

	private Container next(ObjectSet result) {
		final Iterator iterator = result.iterator();
		if (iterator.hasNext()) {
			return (Container) iterator.next();
		}
		return null;
	}
}
