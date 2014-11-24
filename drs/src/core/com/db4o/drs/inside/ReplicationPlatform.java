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
package com.db4o.drs.inside;

import java.util.*;

import com.db4o.reflect.*;

/**
 * Platform dependent code goes here to minimize manually
 * converted code.
 * 
 * @sharpen.ignore
 */
public class ReplicationPlatform {

	public static void copyCollectionState(Object original, Object destination, CounterpartFinder counterpartFinder) {
		Collection originalCollection = (Collection) original;
		Collection destinationCollection = (Collection) destination;
		destinationCollection.clear();
		Iterator it = originalCollection.iterator();
		while (it.hasNext()) {
			Object element = it.next();
			Object counterpart = counterpartFinder.findCounterpart(element);
			destinationCollection.add(counterpart);
		}
	}

	public static Collection emptyCollectionClone(CollectionSource sourceProvider, Collection original) {
		
		boolean providerSpecific = sourceProvider.isProviderSpecific(original);
		
		if (original instanceof ArrayList || (providerSpecific && original instanceof List)) {
			return new ArrayList(original.size());
		}
		if (original instanceof HashSet || (providerSpecific && original instanceof Set)) {
			return new HashSet(original.size());
		}
		 
		return null;
	}

	public static boolean isValueType(Object o) {
		return false;
	}
	
	private static final Class[] _builtinCollectionClasses = new Class[] {
		AbstractList.class,
		AbstractSet.class,
		AbstractQueue.class,
	};

	static boolean isBuiltinCollectionClass(ReplicationReflector reflector, ReflectClass claxx) {
		for (Class c : _builtinCollectionClasses) {
			if (reflector.forClass(c).isAssignableFrom(claxx)) {
				return true;
			}
		}
		// FIXME: Absurdly embarrassing and hackish way of getting the rdbms collection tests to pass
		// we'll review it with more time later
		return claxx.getName().startsWith("org.hibernate.collection.");
	}
}
