/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class QueryConsistencyTestCase : AbstractDb4oTestCase, IOptOutAllButNetworkingCS
	{
		public static void Main(string[] args)
		{
			new _Db4oTestSuite_14().RunAll();
		}

		private sealed class _Db4oTestSuite_14 : Db4oTestSuite
		{
			public _Db4oTestSuite_14()
			{
			}

			protected override Type[] TestCases()
			{
				return new Type[] { typeof(QueryConsistencyTestCase) };
			}
		}

		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.OptimizeNativeQueries(false);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new QueryConsistencyTestCase.Item(42));
		}

		public virtual void TestDelete()
		{
			QueryConsistencyTestCase.Item found = SodaQueryForItem(42);
			Assert.AreEqual(42, found._id);
			Db().Delete(found);
			Assert.IsNull(SodaQueryForItem(42));
			Assert.IsNull(NativeQueryForItem(42));
			Db().Commit();
			Assert.IsNull(SodaQueryForItem(42));
			Assert.IsNull(NativeQueryForItem(42));
		}

		public virtual void TestUpdate()
		{
			QueryConsistencyTestCase.Item found = SodaQueryForItem(42);
			Assert.AreEqual(42, found._id);
			Assert.AreSame(found, NativeQueryForItem(42));
			found._id = 21;
			Assert.IsNull(SodaQueryForItem(21));
			Assert.AreSame(found, SodaQueryForItem(42));
			Assert.AreSame(found, NativeQueryForItem(42));
			Store(found);
			Assert.AreSame(found, SodaQueryForItem(21));
			Assert.AreEqual(21, found._id);
			Assert.AreSame(found, NativeQueryForItem(21));
			Assert.AreEqual(21, found._id);
			Db().Commit();
			Assert.AreSame(found, NativeQueryForItem(21));
		}

		private QueryConsistencyTestCase.Item NativeQueryForItem(int id)
		{
			IObjectSet result = Db().Query(new QueryConsistencyTestCase.ItemById(id));
			return ((QueryConsistencyTestCase.Item)FirstOrNull(result));
		}

		[System.Serializable]
		public sealed class ItemById : Predicate
		{
			public int _id;

			public ItemById(int id)
			{
				_id = id;
			}

			public bool Match(QueryConsistencyTestCase.Item candidate)
			{
				return candidate._id == _id;
			}
		}

		private QueryConsistencyTestCase.Item SodaQueryForItem(int id)
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(QueryConsistencyTestCase.Item));
			q.Descend("_id").Constrain(id).Equal();
			return ((QueryConsistencyTestCase.Item)FirstOrNull(q.Execute()));
		}

		private object FirstOrNull(IObjectSet result)
		{
			return result.HasNext() ? result.Next() : null;
		}
	}
}
#endif // !SILVERLIGHT
