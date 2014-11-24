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
package com.db4o.drs.foundation;

import java.util.*;

import com.db4o.drs.inside.*;
import com.db4o.foundation.*;

/**
 * @sharpen.ignore
 */
public class GenericObjectReferenceMap<T extends ReplicationReference> {
	
	private Map<Object, T> _objectToReplicationReference;
	
	private Map<DrsUUID, T> _uuidToReplicationReference;

	public GenericObjectReferenceMap() {
		init();
	}

	public final T get(Object obj) {
		return _objectToReplicationReference.get(obj);
	}

	public T getByUUID(DrsUUID uuid) {
		return _uuidToReplicationReference.get(uuid);
	}
	
	public final void put(T replicationReference) {
		if(replicationReference == null){
			throw new IllegalArgumentException();
		}
		if (_objectToReplicationReference.containsKey(replicationReference.object())){
			throw new RuntimeException("key already existed");
		}
		_objectToReplicationReference.put(replicationReference.object(), replicationReference);
		_uuidToReplicationReference.put(replicationReference.uuid(), replicationReference);
	}

	public String toString() {
		return _objectToReplicationReference.toString();
	}

	public final void visitEntries(Visitor4 visitor) {
		Iterator i = _objectToReplicationReference.values().iterator();
		while (i.hasNext())
			visitor.visit(i.next());
	}

	public void init() {
		_objectToReplicationReference = new IdentityHashMap();
		_uuidToReplicationReference = new HashMap<DrsUUID, T>();
	}
	
	public void clear(){
		init();
	}
	
}


