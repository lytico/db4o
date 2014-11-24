/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
using System;
using System.Diagnostics;
#if !CF && !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using System.Linq;
using Db4objects.Db4o.Linq;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class QueryMonitoringSupportTestCaseBase : PerformanceCounterTestCaseBase
	{

		protected void ExecuteOptimizedLinq()
		{
			var found = (from Item item in Db()
			             where item.id == 42
			             select item).ToArray();
		}

		protected void ExecuteUnoptimizedLinq()
		{
			var found = (from Item item in Db()
			             where item.GetType() == typeof(Item)
			             select item).ToArray();
		}

		protected void AssertCounter(PerformanceCounter performanceCounter, Action4 action)
		{
			using (PerformanceCounter counter = performanceCounter)
			{
				Assert.AreEqual(0, counter.RawValue);

				for (int i = 0; i < 3; ++i)
				{
					action();
					Assert.AreEqual(i + 1, counter.RawValue);
				}
			}
		}

		protected void ExecuteOptimizedNQ()
		{
			ExecuteOptimizedNQ(Db());
		}

		protected void ExecuteOptimizedNQ(IObjectContainer container)
		{
			Predicate<Item> match = delegate(Item item) { return item.id == 42; };
			container.Query(match);
		}

		protected void ExecuteUnoptimizedNQ()
		{
			Db().Query<Item>(delegate(Item item) { return item.GetType() == typeof (Item); });
		}

		public class Item
		{
			public int id;
		}
	}
}
#endif