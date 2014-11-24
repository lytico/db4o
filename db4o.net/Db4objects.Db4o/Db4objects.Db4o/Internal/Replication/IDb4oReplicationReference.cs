/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Internal.Replication
{
	/// <exclude></exclude>
	public interface IDb4oReplicationReference
	{
		Db4oDatabase SignaturePart();

		long LongPart();

		long Version();
	}
}
