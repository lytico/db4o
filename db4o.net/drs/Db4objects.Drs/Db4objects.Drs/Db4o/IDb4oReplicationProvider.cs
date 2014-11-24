/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Replication;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Db4o
{
	public interface IDb4oReplicationProvider : ITestableReplicationProvider, IDb4oReplicationReferenceProvider
		, ITestableReplicationProviderInside
	{
		IExtObjectContainer GetObjectContainer();
	}
}
