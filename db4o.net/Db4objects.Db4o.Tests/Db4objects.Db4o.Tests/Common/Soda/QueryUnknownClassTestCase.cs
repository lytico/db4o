/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class QueryUnknownClassTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
			public int _id;

			public Data(int id)
			{
				_id = id;
			}
		}

		public class DataHolder
		{
			public ArrayList _data;

			public DataHolder(object data)
			{
				_data = new ArrayList();
				_data.Add(data);
			}
		}

		public class Unrelated
		{
			public int _uid;

			public Unrelated(int id)
			{
				_uid = id;
			}
		}

		public virtual void TestQueryUnknownClass()
		{
			IQuery query = NewQuery(typeof(QueryUnknownClassTestCase.Data));
			query.Descend("_id").Constrain(42);
			IObjectSet result = query.Execute();
			Assert.AreEqual(0, result.Count);
		}

		public virtual void TestQueryUnknownClassInUnknownCollection()
		{
			IQuery query = NewQuery(typeof(QueryUnknownClassTestCase.DataHolder));
			query.Descend("_data").Descend("_id").Constrain(42);
			IObjectSet result = query.Execute();
			Assert.AreEqual(0, result.Count);
		}

		public virtual void _testQueryUnknownClassInCollection()
		{
			Store(new QueryUnknownClassTestCase.DataHolder(new QueryUnknownClassTestCase.Unrelated
				(42)));
			IQuery query = NewQuery(typeof(QueryUnknownClassTestCase.DataHolder));
			query.Descend("_data").Descend("_id").Constrain(42);
			IObjectSet result = query.Execute();
			Assert.AreEqual(0, result.Count);
		}

		public virtual void _testQueryUnknownClassInCollectionConjunction()
		{
			Store(new QueryUnknownClassTestCase.DataHolder(new QueryUnknownClassTestCase.Unrelated
				(42)));
			IQuery query = NewQuery(typeof(QueryUnknownClassTestCase.DataHolder));
			query.Descend("_data").Descend("_id").Constrain(42).And(query.Descend("_data").Descend
				("_uid").Constrain(42));
			IObjectSet result = query.Execute();
			Assert.AreEqual(0, result.Count);
		}

		public virtual void TestQueryUnknownClassInCollectionDisjunction()
		{
			Store(new QueryUnknownClassTestCase.DataHolder(new QueryUnknownClassTestCase.Unrelated
				(42)));
			IQuery query = NewQuery(typeof(QueryUnknownClassTestCase.DataHolder));
			query.Descend("_data").Descend("_id").Constrain(42).Or(query.Descend("_data").Descend
				("_uid").Constrain(42));
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
		}
	}
}
