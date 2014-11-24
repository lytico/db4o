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

import com.db4o.drs.foundation.*;

public class ReplicationReferenceImpl implements ReplicationReference {
	
	private boolean _objectIsNew;

	private final Object _obj;

	private final DrsUUID _uuid;

	private final long _version;

	private Object _counterPart;

	private boolean _markedForReplicating;

	private boolean _markedForDeleting;

	public ReplicationReferenceImpl(Object obj, DrsUUID uuid, long version) {
		this._obj = obj;
		this._uuid = uuid;
		this._version = version;
	}

	public final Object counterpart() {
		return _counterPart;
	}

	public final boolean equals(Object o) {
		if (this == o) return true;
		if (o == null || o.getClass() != getClass()) return false;
		final ReplicationReferenceImpl other = (ReplicationReferenceImpl) o;
		return _version == other._version && _uuid.equals(other._uuid);
	}

	public final int hashCode() {
		return 29 * _uuid.hashCode() + (int) (_version ^ (_version >>> 32));
	}

	public boolean isCounterpartNew() {
		return _objectIsNew;
	}

	public final boolean isMarkedForDeleting() {
		return _markedForDeleting;
	}

	public final boolean isMarkedForReplicating() {
		return _markedForReplicating;
	}

	public void markCounterpartAsNew() {
		_objectIsNew = true;
	}

	public final void markForDeleting() {
		_markedForDeleting = true;
	}

	public final void markForReplicating(boolean flag) {
		_markedForReplicating = flag;
	}

	public final Object object() {
		return _obj;
	}

	public final void setCounterpart(Object obj) {
		_counterPart = obj;
	}

	public String toString() {
		return "ReplicationReferenceImpl{" +
				"_objectIsNew=" + _objectIsNew +
				", _obj=" + _obj +
				", _uuid=" + _uuid +
				", _version=" + _version +
				", _counterPart=" + _counterPart +
				", _markedForReplicating=" + _markedForReplicating +
				", _markedForDeleting=" + _markedForDeleting +
				'}';
	}

	public final DrsUUID uuid() {
		return _uuid;
	}

	public final long version() {
		return _version;
	}
}