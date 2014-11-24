/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class TransactionContext
	{
		public readonly Transaction _transaction;

		public readonly object _object;

		public TransactionContext(Transaction transaction, object obj)
		{
			_transaction = transaction;
			_object = obj;
		}
	}
}
