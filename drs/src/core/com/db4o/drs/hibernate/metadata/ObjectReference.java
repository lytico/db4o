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
package com.db4o.drs.hibernate.metadata;

/**
 * Holds metadata of a persisted object.
 * 
 * @author Albert Kwan
 *
 * @version 1.1
 * @since dRS 1.2
 */
public class ObjectReference {
	public static class Table {
		public static final String NAME = "drs_objects";
	}
	
	public static class Fields {
		public static final String UUID = "uuid";
		public static final String VERSION = "modified";
		public static final String TYPED_ID = "typedId";
		public static final String CLASS_NAME = "className";
	}
	
	/**
	 * Fully qualified class name of the referenced object.
	 */
	private String className;

	/**
	 * The identifier of the referenced object in Hibernate. 
	 * 
	 * @see org.hibernate.Session#getIdentifier(Object refObj)
	 */
	private long typedId;

	/**
	 * The UUID of the referenced object.
	 * 
	 * @see Uuid
	 */
	private Uuid uuid;

	/**
	 * The version number of the referenced object.
	 */
	private long modified;
	
	public ObjectReference() {}

	public String getClassName() {
		return className;
	}

	public void setClassName(String className) {
		this.className = className;
	}

	public long getTypedId() {
		return typedId;
	}

	public void setTypedId(long objectId) {
		this.typedId = objectId;
	}

	public Uuid getUuid() {
		return uuid;
	}

	public void setUuid(Uuid uuid) {
		this.uuid = uuid;
	}

	public long getModified() {
		return modified;
	}

	public void setModified(long version) {
		this.modified = version;
	}
}
