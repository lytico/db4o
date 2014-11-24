/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq;

using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests
{
	public class CountTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name;
			public int Age;
		}

		protected override void Store()
		{
			Store(new Person { Name = "Malkovitch", Age = 24 });
			Store(new Person { Name = "Malkovitch", Age = 20 });
			Store(new Person { Name = "Malkovitch", Age = 25 });
			Store(new Person { Name = "Malkovitch", Age = 32 });
			Store(new Person { Name = "Malkovitch", Age = 7 });
		}

		public void TestOptimizedCount()
		{
			AssertQuery("(Person)",
				delegate
				{
					var johns = from Person p in Db()
								select p;

					Assert.AreEqual(5, johns.Count());
				});
		}
	}
}
