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
package com.db4o.drs.test.foundation;

import com.db4o.drs.foundation.ObjectSetCollection4Facade;
import com.db4o.foundation.Collection4;

import db4ounit.*;

/**
 * @sharpen.ignore
 */
public class ObjectSetCollection4FacadeTestCase implements TestCase {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(ObjectSetCollection4FacadeTestCase.class).run();
	}
	
	public void testEmpty() {
		ObjectSetCollection4Facade facade = new ObjectSetCollection4Facade(new Collection4());
		Assert.isFalse(facade.hasNext());
		Assert.isFalse(facade.hasNext());
	}
	
	public void testIteration() {
		Collection4 collection = new Collection4();		
		collection.add("bar");
		collection.add("foo");
		
		ObjectSetCollection4Facade facade = new ObjectSetCollection4Facade(collection);
		Assert.isTrue(facade.hasNext());
		Assert.areEqual("bar", facade.next());
		Assert.isTrue(facade.hasNext());
		Assert.areEqual("foo", facade.next());
		Assert.isFalse(facade.hasNext());
		
		facade.reset();
		
		Assert.areEqual("bar", facade.next());
		Assert.areEqual("foo", facade.next());
		Assert.isFalse(facade.hasNext());
	}

}
