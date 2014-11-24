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
 * Uniquely identifies a persisted object.
 * 
 * @author Albert Kwan
 *
 * @version 1.2
 * @since dRS 1.0
 */
public class Uuid {
	public static class Table {
		public static final String CREATED = "created";
		public static final String PROVIDER = "provider_id";
	}
	
	public static class Fields {
		public static final String CREATED = "created";
		public static final String PROVIDER = "provider";	
	}
	
	/**
	 * An id that is unique across types within a provider.
	 */
	private long created;

	/**
	 * The provider that orginates this id.
	 */
	private ProviderSignature provider;
	
	public Uuid() {}

	@Override
	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + (int) (created ^ (created >>> 32));
		result = PRIME * result + ((provider == null) ? 0 : provider.hashCode());
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
		final Uuid other = (Uuid) obj;
		if (created != other.created)
			return false;
		if (provider == null) {
			if (other.provider != null)
				return false;
		} else if (!provider.equals(other.provider))
			return false;
		return true;
	}

	public long getCreated() {
		return created;
	}

	public void setCreated(long created) {
		this.created = created;
	}

	public ProviderSignature getProvider() {
		return provider;
	}

	public void setProvider(ProviderSignature provider) {
		this.provider = provider;
	}
}
