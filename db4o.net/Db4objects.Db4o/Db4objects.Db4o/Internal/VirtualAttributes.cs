/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class VirtualAttributes : IShallowClone
	{
		public Db4oDatabase i_database;

		public long i_version;

		public long i_uuid;

		// FIXME: should be named "uuidLongPart" or even better "creationTime" 
		public virtual object ShallowClone()
		{
			VirtualAttributes va = new VirtualAttributes();
			va.i_database = i_database;
			va.i_version = i_version;
			va.i_uuid = i_uuid;
			return va;
		}

		internal virtual bool SuppliesUUID()
		{
			return i_database != null && i_uuid != 0;
		}
	}
}
