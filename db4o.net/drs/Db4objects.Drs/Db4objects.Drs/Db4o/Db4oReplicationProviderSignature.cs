/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Db4o
{
	/// <exclude></exclude>
	public class Db4oReplicationProviderSignature : IReadonlyReplicationProviderSignature
	{
		private readonly Db4oDatabase _delegate;

		public Db4oReplicationProviderSignature(Db4oDatabase delegate_)
		{
			_delegate = delegate_;
		}

		public virtual long GetId()
		{
			return 0;
		}

		public virtual byte[] GetSignature()
		{
			return _delegate.GetSignature();
		}

		public virtual long GetCreated()
		{
			return _delegate.GetCreationTime();
		}
	}
}
