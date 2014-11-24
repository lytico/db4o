/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class QueryingForAllObjectsTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < 3; i++)
			{
				Store(new QueryingForAllObjectsTestCase.Item());
			}
		}

		public virtual void TestConstrainObjectClass()
		{
			IObjectSet objectSet = Db().Query(typeof(object));
			Assert.AreEqual(3, objectSet.Count);
		}

		public virtual void TestConstrainByNewObject()
		{
			IObjectSet objectSet = Db().QueryByExample(new object());
			Assert.AreEqual(3, objectSet.Count);
		}
	}
}
