/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class DatabaseContext
	{
		public readonly IObjectContainer _db;

		public readonly object _unactivated;

		public DatabaseContext(IObjectContainer db, object unactivated)
		{
			_db = db;
			_unactivated = unactivated;
		}

		public virtual bool StorageIsClosed()
		{
			return ((ExceptionSimulatingStorage)_db.Ext().Configure().Storage).IsClosed();
		}
	}
}
