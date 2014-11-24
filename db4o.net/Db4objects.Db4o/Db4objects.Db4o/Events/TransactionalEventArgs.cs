/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Events
{
	public class TransactionalEventArgs : EventArgs
	{
		private readonly Db4objects.Db4o.Internal.Transaction _transaction;

		public TransactionalEventArgs(Db4objects.Db4o.Internal.Transaction transaction)
		{
			_transaction = transaction;
		}

		public virtual object Transaction()
		{
			return _transaction;
		}

		public virtual IObjectContainer ObjectContainer()
		{
			return _transaction.ObjectContainer();
		}
	}
}
