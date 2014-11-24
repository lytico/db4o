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

import com.db4o.foundation.Iterators;

import db4ounit.Assert;
import db4ounit.TestCase;

public class Set4Testcase implements TestCase {
	
	public void testSingleElementIteration() {
		Set4 set = newSet("first");
		Assert.areEqual("first", Iterators.next(set.iterator()));
	}

	public void testContainsAll() {
		Set4 set = newSet("42");
		set.add("foo");
		
		Assert.isTrue(set.containsAll(newSet("42")));
		Assert.isTrue(set.containsAll(newSet("foo")));
		Assert.isTrue(set.containsAll(set));
		
		Set4 other = new Set4(set);
		other.add("bar");
		Assert.isFalse(set.containsAll(other));
	}
	
	private Set4 newSet(final String firstElement) {
		Set4 set = new Set4();
		set.add(firstElement);
		return set;
	}

}
