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

import java.util.Arrays;

import com.db4o.drs.inside.ReadonlyReplicationProviderSignature;

/**
 * Uniquely identifies a RDBMS.
 * @author Albert Kwan
 *
 * @see PeerSignature
 * @see MySignature
 * 
 * @version 1.2
 * @since dRS 1.1
 */
public abstract class ProviderSignature 
	implements ReadonlyReplicationProviderSignature {
	
	public static class Fields {
		public static final String ID = "id";
		public static final String SIG = "signature";
	}

	/**
	 * unique identifier for this ProviderSignature.
	 */
	private byte[] signature;

	/**
	 * 1 to 1 identifier of "signature", for space usage optimization. 
	 * Serves as the primary key in relational table.
	 */
	private long id;

	/**
	 * legacy field used by pre-dRS db4o replication code.
	 * @deprecated
	 */
	private long created;
	
	public ProviderSignature() {
		this.created = System.currentTimeMillis();
	}

	public ProviderSignature(byte[] signature) {
		this();
		this.signature = signature;
	}

	@Override
	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + Arrays.hashCode(signature);
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
		final ProviderSignature other = (ProviderSignature) obj;
		if (!Arrays.equals(signature, other.signature))
			return false;
		return true;
	}

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public byte[] getSignature() {
		return signature;
	}

	public void setSignature(byte[] signature) {
		this.signature = signature;
	}

	public long getCreated() {
		return created;
	}

	public void setCreated(long created) {
		this.created = created;
	}
}
