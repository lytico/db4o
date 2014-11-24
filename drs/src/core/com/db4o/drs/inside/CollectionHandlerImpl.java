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

import com.db4o.foundation.*;
import com.db4o.reflect.*;

public class CollectionHandlerImpl implements CollectionHandler {

	private final CollectionHandler _mapHandler;
	private final ReplicationReflector _reflector;

	public CollectionHandlerImpl(ReplicationReflector reflector) {
		_mapHandler = new MapHandler(reflector);
		_reflector = reflector;
	}

	public boolean canHandleClass(ReflectClass claxx) {
		if (_mapHandler.canHandleClass(claxx)) return true;
		return ReplicationPlatform.isBuiltinCollectionClass(_reflector, claxx);
	}

	public boolean canHandle(Object obj) {
		return canHandleClass(_reflector.forObject(obj));
	}

	public boolean canHandleClass(Class c) {
		return canHandleClass(_reflector.forClass(c));
	}

	public Object emptyClone(CollectionSource sourceProvider, Object originalCollection, ReflectClass originalCollectionClass) {
		if (_mapHandler.canHandleClass(originalCollectionClass))
			return _mapHandler.emptyClone(sourceProvider, originalCollection, originalCollectionClass);

		Collection original = (Collection) originalCollection;

		Collection clone = ReplicationPlatform.emptyCollectionClone(sourceProvider, original);
		if (null != clone) return clone;
		
		return _reflector.forClass(original.getClass()).newInstance();
	}

	public Iterator4 iteratorFor(Object collection) {
		if (_mapHandler.canHandleClass(_reflector.forObject(collection)))
			return _mapHandler.iteratorFor(collection);

		Iterable subject = (Iterable) collection;
		return copy(subject).iterator();
	}

	private Collection4 copy(Iterable subject) {
		Collection4 result = new Collection4();
		Iterator it = subject.iterator();
		while (it.hasNext()) result.add(it.next());
		return result;
	}

	public void copyState(Object original, Object destination, CounterpartFinder counterpartFinder) {
		if (_mapHandler.canHandle(original))
			_mapHandler.copyState(original, destination, counterpartFinder);
		else
			ReplicationPlatform.copyCollectionState(original, destination, counterpartFinder);
	}

	public Object cloneWithCounterparts(CollectionSource sourceProvider, Object originalCollection, ReflectClass claxx, CounterpartFinder counterpartFinder) {
		if (_mapHandler.canHandleClass(claxx))
			return _mapHandler.cloneWithCounterparts(sourceProvider, originalCollection, claxx, counterpartFinder);

		Collection original = (Collection) originalCollection;
		Collection result = (Collection) emptyClone(sourceProvider, originalCollection, claxx);

		copyState(original, result, counterpartFinder);

		return result;
	}
}