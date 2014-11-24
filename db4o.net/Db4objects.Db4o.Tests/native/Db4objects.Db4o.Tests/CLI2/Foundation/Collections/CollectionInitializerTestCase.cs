/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation.Collections;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Foundation.Collections
{
	class CollectionInitializerTestCase : ITestCase
	{
		private static readonly object[] Values = new object[] { 10, 20 };

		public void Test()
		{
			object list = new LinkedList<int>();
			ICollectionInitializer initializer = CollectionInitializer.For(list);

			Assert.IsNotNull(initializer);

			foreach(object item in Values)
			{
				initializer.Add(item);
			}

			initializer.FinishAdding();

			Iterator4Assert.AreEqual(Values, ((IEnumerable) list).GetEnumerator());
		}

		public void TestNotACollection()
		{
			Assert.Expect(
				typeof(ArgumentException), 
				delegate
					{
						CollectionInitializer.For(10);
					}
				);
		}
	}
}
