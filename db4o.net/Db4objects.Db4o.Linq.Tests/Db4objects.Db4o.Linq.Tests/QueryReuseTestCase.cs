/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Linq.Tests
{
	public class QueryReuseTestCase : AbstractDb4oLinqTestCase
	{
		public class Item
		{
			public int Id;

			public Item(int id)
			{
				Id = id;
			}
		}

		protected override void Store()
		{	
			Store(new Item(1));
		}

		public void TestQueryCanBeReused()
		{
			// Db().Cast<Item>().Select(item => item.Id)
			//   Db4oQuery() { q => q.Constrain(typeof(Item) }
			//      Db4oProjection() => { query = ...; projection = lambda }
			var query = from Item item in Db() select item.Id;

			AssertItemQuery(query);
			AssertItemQuery(query);
		}

		private void AssertItemQuery(IDb4oLinqQuery<int> query)
		{
			AssertQuery("(Item)", () =>
			{
				AssertSequence(new[] { 1 }, query);
			});
		}

		public void TestQueryCanBeComposed()
		{
		}
	}
}
