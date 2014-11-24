/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Replication;

namespace Db4objects.Db4o.Internal.Replication
{
	/// <exclude></exclude>
	public interface IDb4oReplicationReferenceProvider
	{
		IDb4oReplicationReference ReferenceFor(object obj);
	}
}
