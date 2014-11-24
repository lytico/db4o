/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Drs.Db4o;

namespace Db4objects.Drs.Db4o
{
	public class Db4oClientServerReplicationProvider : Db4oEmbeddedReplicationProvider
	{
		public Db4oClientServerReplicationProvider(IObjectContainer objectContainer) : base
			(objectContainer, "null")
		{
		}

		public Db4oClientServerReplicationProvider(IObjectContainer objectContainer, string
			 name) : base(objectContainer, name)
		{
		}

		protected override void Refresh(object obj)
		{
			_container.Refresh(obj, 1);
		}

		public override void CommitReplicationTransaction()
		{
			StoreReplicationRecord();
			((ClientObjectContainer)_container).CommitReplication(ReplicationRecordId(), _commitTimestamp
				);
		}
	}
}
