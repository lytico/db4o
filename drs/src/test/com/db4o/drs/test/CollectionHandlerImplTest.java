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

import com.db4o.drs.inside.*;
import com.db4o.reflect.Reflector;
import db4ounit.Assert;

public class CollectionHandlerImplTest extends DrsTestCase {

	private CollectionHandlerImpl _collectionHandler;

	public void testVector() {
		Vector vector = new Vector();
		Assert.isTrue(collectionHandler().canHandle(vector));
		Assert.isTrue(collectionHandler().canHandleClass(replicationReflector().forObject(vector)));
		Assert.isTrue(collectionHandler().canHandleClass(Vector.class));
	}

	/**
	 * @sharpen.ignore no AbstractList for you
	 */
	public void testList() {
		List list = new LinkedList();
		Assert.isTrue(collectionHandler().canHandle(list));
		Assert.isTrue(collectionHandler().canHandleClass(replicationReflector().forObject(list)));
		Assert.isTrue(collectionHandler().canHandleClass(AbstractList.class));
	}
	
	/**
	 * @sharpen.ignore no AbstractSet for you
	 */
	public void testSet() {
		Set set = new HashSet();
		Assert.isTrue(collectionHandler().canHandle(set));
		Assert.isTrue(collectionHandler().canHandleClass(replicationReflector().forObject(set)));
		Assert.isTrue(collectionHandler().canHandleClass(AbstractSet.class));
	}

	public void testMap() {
		Map map = new HashMap();
		Assert.isTrue(collectionHandler().canHandle(map));
		Assert.isTrue(collectionHandler().canHandleClass(replicationReflector().forObject(map)));
		Assert.isTrue(collectionHandler().canHandleClass(Map.class));
	}

	public void testString() {
		String str = "abc";
		Assert.isTrue(!collectionHandler().canHandle(str));
		Assert.isTrue(!collectionHandler().canHandleClass(replicationReflector().forObject(str)));
		Assert.isTrue(!collectionHandler().canHandleClass(String.class));
	}
	
	private CollectionHandler collectionHandler() {
		if(_collectionHandler == null) {
			_collectionHandler = new CollectionHandlerImpl(replicationReflector());
		}
		return _collectionHandler;
	}
}
