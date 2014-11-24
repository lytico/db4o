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
package com.db4o.drs.hibernate.impl;

class HibernateObjectId {
	public final long _hibernateId;
	
	public final String _className;
	
	public HibernateObjectId(long hibernateId, String className) {
		this._hibernateId = hibernateId;
		this._className = className;
	}

	@Override
	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + ((_className == null) ? 0 : _className.hashCode());
		result = PRIME * result + (int) (_hibernateId ^ (_hibernateId >>> 32));
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final HibernateObjectId other = (HibernateObjectId) obj;
		if (_className == null) {
			if (other._className != null)
				return false;
		} else if (!_className.equals(other._className))
			return false;
		if (_hibernateId != other._hibernateId)
			return false;
		return true;
	}
}
