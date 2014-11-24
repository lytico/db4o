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

import com.db4o.foundation.Collection4;
import com.db4o.foundation.Iterator4;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.Reflector;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class MapHandler implements CollectionHandler {

	private final ReflectClass _reflectMapClass;
	private final ReplicationReflector _reflector;

	public MapHandler(ReplicationReflector reflector) {
		_reflector = reflector;
		_reflectMapClass = reflector.forClass(Map.class);
	}

	public boolean canHandleClass(ReflectClass claxx) {
		return _reflectMapClass.isAssignableFrom(claxx);
	}

	public boolean canHandle(Object obj) {
		return canHandleClass(_reflector.forObject(obj));
	}

	public boolean canHandleClass(Class c) {
		return canHandleClass(_reflector.forClass(c));
	}

	public Iterator4 iteratorFor(final Object collection) {
		Map map = (Map) collection;
		Collection4 result = new Collection4();

		Iterator it = map.entrySet().iterator();
		while (it.hasNext()) {
			Map.Entry entry = (Map.Entry) it.next();
			result.add(entry.getKey());
			result.add(entry.getValue());
		}

		return result.iterator();
	}

	public Object emptyClone(CollectionSource sourceProvider, Object original, ReflectClass originalCollectionClass) {

		if (sourceProvider.isProviderSpecific(original)
			|| original instanceof HashMap) {
			return new HashMap(((Map) original).size());
		}
		return _reflector.forObject(original).newInstance();
	}
	
	public void copyState(Object original, Object destination, CounterpartFinder counterpartFinder) {

		Map originalMap = (Map) original;
		Map destinationMap = (Map) destination;

		destinationMap.clear();

		Iterator it = originalMap.entrySet().iterator();
		while (it.hasNext()) {
			Map.Entry entry = (Map.Entry) it.next();
			Object keyClone = counterpartFinder.findCounterpart(entry.getKey());
			Object valueClone = counterpartFinder.findCounterpart(entry.getValue());
			destinationMap.put(keyClone, valueClone);
		}

	}

	public Object cloneWithCounterparts(CollectionSource sourceProvider, Object originalMap, ReflectClass claxx, CounterpartFinder elementCloner) {
		Map original = (Map) originalMap;

		Map result = (Map) emptyClone(sourceProvider, original, claxx);

		copyState(original, result, elementCloner);

		return result;
	}
}
