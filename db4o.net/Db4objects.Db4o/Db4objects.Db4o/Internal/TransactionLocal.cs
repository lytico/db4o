/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <summary>A transaction local variable.</summary>
	/// <remarks>A transaction local variable.</remarks>
	/// <seealso cref="Transaction.Get(TransactionLocal)">Transaction.Get(TransactionLocal)
	/// 	</seealso>
	public class TransactionLocal
	{
		public virtual object InitialValueFor(Transaction transaction)
		{
			return null;
		}
	}
}
