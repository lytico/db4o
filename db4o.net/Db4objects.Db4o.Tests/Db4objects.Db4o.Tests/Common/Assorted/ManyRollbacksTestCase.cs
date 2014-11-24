/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class ManyRollbacksTestCase : AbstractDb4oTestCase
	{
		private const int Count = 900;

		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			for (int i = 0; i < Count; i++)
			{
				Store(new ManyRollbacksTestCase.Item());
				Db().Rollback();
			}
			Reopen();
		}
	}
}
