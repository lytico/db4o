/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class TypeHandlerTestCaseBase : AbstractDb4oTestCase
	{
		protected virtual void DoTestStoreObject(object storedItem)
		{
			Db().Store(storedItem);
			Db().Purge(storedItem);
			object readItem = RetrieveOnlyInstance(storedItem.GetType());
			Assert.AreNotSame(storedItem, readItem);
			Assert.AreEqual(storedItem, readItem);
		}
	}
}
