/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Internal.Query;

namespace Db4objects.Db4o.Tests.Common.Internal.Query
{
	public class SodaQueryComparatorBenchmark
	{
		private const int ObjectCount = 10000;

		private const int Iterations = 10;

		public class Item
		{
			public Item(int id, string name, SodaQueryComparatorBenchmark.ItemChild child)
			{
				this.id = id;
				this.name = name;
				this.child = child;
			}

			public int id;

			public string name;

			public SodaQueryComparatorBenchmark.ItemChild child;
		}

		public class ItemChild
		{
			public ItemChild(string name)
			{
				this.name = name;
			}

			public string name;
		}

		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			for (int i = 0; i < 2; ++i)
			{
				BenchmarkOneField();
				BenchmarkTwoFields();
			}
		}

		private static void BenchmarkTwoFields()
		{
			long sqc = Time(new _IProcedure4_55());
			Sharpen.Runtime.Out.WriteLine(" SQC(2): " + sqc + "ms");
			long soda = Time(new _IProcedure4_77());
			Sharpen.Runtime.Out.WriteLine("SODA(2): " + soda + "ms");
		}

		private sealed class _IProcedure4_55 : IProcedure4
		{
			public _IProcedure4_55()
			{
			}

			public void Apply(object container)
			{
				LocalObjectContainer localContainer = (LocalObjectContainer)((IObjectContainer)container
					);
				SodaQueryComparator comparator = new SodaQueryComparator(localContainer, typeof(SodaQueryComparatorBenchmark.Item
					), new SodaQueryComparator.Ordering[] { new SodaQueryComparator.Ordering(SodaQueryComparator.Direction
					.Ascending, new string[] { "name" }), new SodaQueryComparator.Ordering(SodaQueryComparator.Direction
					.Descending, new string[] { "child", "name" }) });
				IQuery query = ((IObjectContainer)container).Query();
				query.Constrain(typeof(SodaQueryComparatorBenchmark.Item));
				IList sortedIds = comparator.Sort(query.Execute().Ext().GetIDs());
				for (IEnumerator idIter = sortedIds.GetEnumerator(); idIter.MoveNext(); )
				{
					int id = ((int)idIter.Current);
					Assert.IsNull(localContainer.GetActivatedObjectFromCache(localContainer.Transaction
						, id));
				}
			}
		}

		private sealed class _IProcedure4_77 : IProcedure4
		{
			public _IProcedure4_77()
			{
			}

			public void Apply(object container)
			{
				IQuery query = ((IObjectContainer)container).Query();
				query.Constrain(typeof(SodaQueryComparatorBenchmark.Item));
				query.Descend("name").OrderAscending();
				query.Descend("child").Descend("name").OrderDescending();
				SodaQueryComparatorBenchmark.ConsumeAll(query.Execute());
			}
		}

		private static void BenchmarkOneField()
		{
			long sqc = Time(new _IProcedure4_91());
			Sharpen.Runtime.Out.WriteLine(" SQC(1): " + sqc + "ms");
			long soda = Time(new _IProcedure4_112());
			Sharpen.Runtime.Out.WriteLine("SODA(1): " + soda + "ms");
		}

		private sealed class _IProcedure4_91 : IProcedure4
		{
			public _IProcedure4_91()
			{
			}

			public void Apply(object container)
			{
				LocalObjectContainer localContainer = (LocalObjectContainer)((IObjectContainer)container
					);
				SodaQueryComparator comparator = new SodaQueryComparator(localContainer, typeof(SodaQueryComparatorBenchmark.Item
					), new SodaQueryComparator.Ordering[] { new SodaQueryComparator.Ordering(SodaQueryComparator.Direction
					.Ascending, new string[] { "name" }) });
				IQuery query = ((IObjectContainer)container).Query();
				query.Constrain(typeof(SodaQueryComparatorBenchmark.Item));
				IList sortedIds = comparator.Sort(query.Execute().Ext().GetIDs());
				for (IEnumerator idIter = sortedIds.GetEnumerator(); idIter.MoveNext(); )
				{
					int id = ((int)idIter.Current);
					Assert.IsNull(localContainer.GetActivatedObjectFromCache(localContainer.Transaction
						, id));
				}
			}
		}

		private sealed class _IProcedure4_112 : IProcedure4
		{
			public _IProcedure4_112()
			{
			}

			public void Apply(object container)
			{
				IQuery query = ((IObjectContainer)container).Query();
				query.Constrain(typeof(SodaQueryComparatorBenchmark.Item));
				query.Descend("name").OrderAscending();
				SodaQueryComparatorBenchmark.ConsumeAll(query.Execute());
			}
		}

		protected static void ConsumeAll(IEnumerable items)
		{
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				object item = itemIter.Current;
				Assert.IsNotNull(item);
			}
		}

		private static long Time(IProcedure4 procedure4)
		{
			PagingMemoryStorage storage = new PagingMemoryStorage();
			StoreItems(storage);
			StopWatch stopWatch = new AutoStopWatch();
			for (int i = 0; i < Iterations; ++i)
			{
				ApplyProcedure(storage, procedure4);
			}
			return stopWatch.Peek();
		}

		private static void ApplyProcedure(PagingMemoryStorage storage, IProcedure4 procedure4
			)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = storage;
			IEmbeddedObjectContainer container = Db4oEmbedded.OpenFile(config, "benchmark.db4o"
				);
			try
			{
				procedure4.Apply(container);
			}
			finally
			{
				container.Close();
			}
		}

		private static void StoreItems(PagingMemoryStorage storage)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = storage;
			IEmbeddedObjectContainer container = Db4oEmbedded.OpenFile(config, "benchmark.db4o"
				);
			try
			{
				for (int i = 0; i < ObjectCount; ++i)
				{
					container.Store(new SodaQueryComparatorBenchmark.Item(i, "Item " + i, new SodaQueryComparatorBenchmark.ItemChild
						("Child " + i)));
				}
			}
			finally
			{
				container.Close();
			}
		}
	}
}
