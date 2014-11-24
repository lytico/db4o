/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using System.Linq;
using Db4objects.Db4o.Config;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests.QueryOperators
{
	public partial class SkipTestUnit : AbstractDb4oLinqTestCase
	{
		private const int TotalToSkip = 3;
		
		private static Item[] Items()
		{
			return new[]
			       	{
			       		new Item { Id = 1, Name = "foo"},
			       		new Item { Id = 2, Name = "bar"},
			       		new Item { Id = 3, Name = "baz"},
			       		new Item { Id = 4, Name = "foo.bar"},
			       		new Item { Id = 5, Name = "boo"},
					};
		}

		protected override void Configure(IConfiguration config)
		{
			config.Queries().EvaluationMode(EvaluationModeToTest());
		}

		public void TestComposedQuery()
		{
			RegisterForActivationEvents();

			var originalQuery = from Item item in Db()
								select item.Id;

			var skippedQuery = originalQuery.Skip(1);

			var expectedCount = Items().Length;
			
			Assert.AreEqual(expectedCount - 1, skippedQuery.Count());
			AssertActivationCount(expectedCount - 1);
			
			Assert.AreEqual(expectedCount, originalQuery.Count());
			
		}

		public void TestSkipedItemsAreNotActivated()
		{
			RegisterForActivationEvents();

			var expected = (from Item item in Items()
							where item.Id >= 2
							select item).Skip(TotalToSkip);

			var actual = (from Item item in Db()
						  where item.Id >= 2
						  select item).Skip(TotalToSkip);

			AssertQuery(actual, "(Item(Id >= 2))", expected);

			AssertActivationCount(expected.Count());
		}

		//TODO: Write the test!
		public void _TestSkippingOnGroupBy()
        {
				var result = (from Person p in Db()
							group new { TheSource = p, TheSourceName = p.Name} by p.Name).Skip(10);
        }

		public void TestComplexProjectionsDoesNotLeadsToActivationWhenSkiping()
		{
			RegisterForActivationEvents();

			var expected = (from Item item in Items()
							where item.Id >= 2
							select new { item.Name } ).Skip(TotalToSkip);

			var actual = (from Item item in Db()
						  where item.Id >= 2
						  select new { item.Name } ).Skip(TotalToSkip);

			AssertQuery(actual, "(Item(Id >= 2))", expected);

			AssertActivationCount(expected.Count());
		}
	}
}
