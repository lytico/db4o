/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o
{
	/// <exclude></exclude>
	public interface ITransactionAware
	{
		void SetTrans(Transaction a_trans);
	}
}
