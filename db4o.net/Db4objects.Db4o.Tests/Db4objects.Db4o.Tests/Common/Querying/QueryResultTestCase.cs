/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public abstract class QueryResultTestCase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutDefragSolo
	{
		private static readonly int[] Values = new int[] { 1, 5, 6, 7, 9 };

		private readonly int[] itemIds = new int[Values.Length];

		private int idForGetAll;

		protected override void Configure(IConfiguration config)
		{
			IndexField(config, typeof(QueryResultTestCase.Item), "foo");
		}

		public virtual void TestClassQuery()
		{
			AssertIDs(ClassOnlyQuery(), itemIds);
		}

		public virtual void TestGetAll()
		{
			AbstractQueryResult queryResult = NewQueryResult();
			queryResult.LoadFromClassIndexes(Container().ClassCollection().Iterator());
			int[] ids = IntArrays4.Concat(itemIds, new int[] { idForGetAll });
			AssertIDs(queryResult, ids, true);
		}

		public virtual void TestIndexedFieldQuery()
		{
			IQuery query = NewItemQuery();
			query.Descend("foo").Constrain(6).Smaller();
			IQueryResult queryResult = ExecuteQuery(query);
			AssertIDs(queryResult, new int[] { itemIds[0], itemIds[1] });
		}

		public virtual void TestNonIndexedFieldQuery()
		{
			IQuery query = NewItemQuery();
			query.Descend("bar").Constrain(6).Smaller();
			IQueryResult queryResult = ExecuteQuery(query);
			AssertIDs(queryResult, new int[] { itemIds[0], itemIds[1] });
		}

		private IQueryResult ClassOnlyQuery()
		{
			AbstractQueryResult queryResult = NewQueryResult();
			queryResult.LoadFromClassIndex(ClassMetadata());
			return queryResult;
		}

		private Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return ClassMetadataFor(typeof(QueryResultTestCase.Item));
		}

		private IQueryResult ExecuteQuery(IQuery query)
		{
			AbstractQueryResult queryResult = NewQueryResult();
			queryResult.LoadFromQuery((QQuery)query);
			return queryResult;
		}

		private void AssertIDs(IQueryResult queryResult, int[] expectedIDs)
		{
			AssertIDs(queryResult, expectedIDs, false);
		}

		private void AssertIDs(IQueryResult queryResult, int[] expectedIDs, bool ignoreUnexpected
			)
		{
			ExpectingVisitor expectingVisitor = new ExpectingVisitor(IntArrays4.ToObjectArray
				(expectedIDs), false, ignoreUnexpected);
			IIntIterator4 i = queryResult.IterateIDs();
			while (i.MoveNext())
			{
				expectingVisitor.Visit(i.CurrentInt());
			}
			expectingVisitor.AssertExpectations();
		}

		protected virtual IQuery NewItemQuery()
		{
			return NewQuery(typeof(QueryResultTestCase.Item));
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			StoreItems(Values);
			QueryResultTestCase.ItemForGetAll ifga = new QueryResultTestCase.ItemForGetAll();
			Store(ifga);
			idForGetAll = (int)Db().GetID(ifga);
		}

		protected virtual void StoreItems(int[] foos)
		{
			for (int i = 0; i < foos.Length; i++)
			{
				QueryResultTestCase.Item item = new QueryResultTestCase.Item(foos[i]);
				Store(item);
				itemIds[i] = (int)Db().GetID(item);
			}
		}

		public class Item
		{
			public int foo;

			public int bar;

			public Item()
			{
			}

			public Item(int foo_)
			{
				foo = foo_;
				bar = foo;
			}
		}

		public class ItemForGetAll
		{
		}

		protected abstract AbstractQueryResult NewQueryResult();
	}
}
