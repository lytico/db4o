/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o.Events;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
	/// <summary>
	///   Tests the .Skip operation on db4o queries.
	/// </summary>
	public class SkipTestCase : AbstractDb4oLinqTestCase
	{
		private const int NumberOfStoredItems = 32;

		protected override void Store()
		{
			for (int i = 0; i < NumberOfStoredItems; i++)
			{
				Db().Store(new Item(i));
			}
		}

		public void TestSkipReturnsAll()
		{
			ExpectNumberOfItemsActivated(NumberOfStoredItems,
										 query => query.Skip(0));
		}

		public void TestSkipReturnsNone()
		{
			ExpectNumberOfItemsActivated(0,
										 query => query.Skip(NumberOfStoredItems));
		}

		public void TestSkipOne()
		{
			ExpectNumberOfItemsActivated(NumberOfStoredItems - 1,
										 query => query.Skip(1));
		}

		public void TestSkipNearlyAll()
		{
			ExpectNumberOfItemsActivated(1,
										 query => query.Skip(NumberOfStoredItems - 1));
		}

		public void TestReturnsRightItems()
		{
			const int skipDistance = NumberOfStoredItems / 2;
			var all = RegularQuery(Db()).ToList();

			ExpectNumberOfItemsActivated(NumberOfStoredItems - skipDistance,
										 query =>
										 {
											 var result = query.Skip(skipDistance);
											 Assert.IsTrue(
												 all.AsEnumerable().Skip(skipDistance).SequenceEqual(result));
											 return result;
										 });
		}

		public void TestCanBeNegative()
		{
			ExpectNumberOfItemsActivated(NumberOfStoredItems,
										 query => query.Skip(-1));
		}

		public void TestCanBeToBig()
		{
			ExpectNumberOfItemsActivated(0,
										 query => query.Skip(NumberOfStoredItems + 1));
		}

		public void TestWorksWithSelect()
		{
			ExpectNumberOfItemsActivated(2,
										 query =>
										 {
											 var result = query.Select(i => i.Number).Skip(NumberOfStoredItems - 2);
											 return result.Select(i => new Item(i));
										 });
		}


		public void TestWorksWithUnoptimizedQuery()
		{
			ExpectNumberOfItemsActivated(NumberOfStoredItems,
										 (IObjectContainer db) => (from Item i in db
																   where i.ToString()[0] != 'x'
																   select i).Skip(0));
		}

#if !CF
		public void TestWorksWithQueryable()
		{
			ExpectNumberOfItemsActivated(1,
							(IObjectContainer db) => (from i in db.AsQueryable<Item>()
													  where i.Number > -1
													  select i).Skip(NumberOfStoredItems - 1));
		}

		public void TestWorksWithQueryableSelect()
		{
			ExpectNumberOfItemsActivated(3,
							db => ( from i in db.AsQueryable<Item>()
								    where i.Number > -1
									select i.Number).Skip(NumberOfStoredItems - 3));
		}
#endif

		private static IDb4oLinqQuery<Item> RegularQuery(IObjectContainer db)
		{
			// The where-clause is required to actually step into the db4o LINQ implementation.
			return (from Item i in db
					where i.Number > -1
					select i);
		}

		private static IDb4oLinqQuery<Item> OrderedQuery(IObjectContainer db)
		{
			return RegularQuery(db).OrderBy(i => i.Number);
		}
		private static IDb4oLinqQuery<Item> OnlyOrder(IObjectContainer db)
		{
			return (from Item i in db
					orderby i.Number
					select i);
		}
		private static IDb4oLinqQuery<Item> PlainQuery(IObjectContainer db)
		{
			return (from Item i in db
					select i);
		}

		private static IEnumerable<Func<IObjectContainer, IDb4oLinqQuery<Item>>> TestQueries()
		{
			yield return RegularQuery;
			yield return OrderedQuery;
			yield return OnlyOrder;
			yield return PlainQuery;
		}

		private void ExpectNumberOfItemsActivated(int maximumItems,
												  Func<IDb4oLinqQuery<Item>, IEnumerable<Item>> testCandidate)
		{


			foreach (var query in TestQueries())
			{
				var prototypeQuery = query;
				ExpectNumberOfItemsActivated(maximumItems,
							   db => testCandidate(prototypeQuery(db)));
			}
		}

		private void ExpectNumberOfItemsActivated<T>(int maximumItems,
												  Func<IObjectContainer, IEnumerable<T>> testCandidate)
		{
			ActivateAtMost(maximumItems,
							db =>
							{
								var result = testCandidate(db);
								Assert.AreEqual(maximumItems, result.Count());
							});
		}

		private void ActivateAtMost(int maximumActivationExpected, Action<IObjectContainer> with)
		{
			var counter = 0;
			Action<object, ObjectInfoEventArgs> counterIncreaser =
				(o, eventArgs) => { counter++; };
			try
			{
				EventRegistry().Activated += counterIncreaser.Invoke;
				with(Db());
				Assert.IsSmallerOrEqual(maximumActivationExpected, counter);
			}
			finally
			{
				EventRegistry().Activated -= counterIncreaser.Invoke;
			}
		}


		public class Item
		{
			public int _number;

			public Item(int number)
			{
				_number = number;
			}

			public int Number
			{
				get { return _number; }
			}
		}
	}
}