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

import com.db4o.drs.ObjectState;

public class ObjectStateImpl implements ObjectState {

	public static final long UNKNOWN = -1;
	
	private Object _object;
	private boolean _isNew;
	private boolean _wasModified;
	private long _modificationDate;

	public Object getObject() {
		return _object;
	}

	public boolean isNew() {
		return _isNew;
	}

	public boolean wasModified() {
		return _wasModified;
	}

	public long modificationDate() {
		return _modificationDate;
	}

	void setAll(Object obj, boolean isNew, boolean wasModified, long modificationDate) {
		_object = obj;
		_isNew = isNew;
		_wasModified = wasModified;
		_modificationDate = modificationDate;
	}


	public String toString() {
		return "ObjectStateImpl{" +
				"_object=" + _object +
				", _isNew=" + _isNew +
				", _wasModified=" + _wasModified +
				", _modificationDate=" + _modificationDate +
				'}';
	}

	public boolean isKnown() {
		return _modificationDate != UNKNOWN;
	}
}
