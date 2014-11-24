/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class IndexedQueriesTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] arguments)
		{
			new IndexedQueriesTestCase().RunSolo();
		}

		public class IndexedQueriesItem
		{
			public string _name;

			public int _int;

			public int _integer;

			public IndexedQueriesItem()
			{
			}

			public IndexedQueriesItem(string name)
			{
				_name = name;
			}

			public IndexedQueriesItem(int int_)
			{
				_int = int_;
				_integer = int_;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			IndexField(config, "_name");
			IndexField(config, "_int");
			IndexField(config, "_integer");
		}

		private void IndexField(IConfiguration config, string fieldName)
		{
			IndexField(config, typeof(IndexedQueriesTestCase.IndexedQueriesItem), fieldName);
		}

		protected override void Store()
		{
			string[] strings = new string[] { "a", "c", "b", "f", "e" };
			for (int i = 0; i < strings.Length; i++)
			{
				Db().Store(new IndexedQueriesTestCase.IndexedQueriesItem(strings[i]));
			}
			int[] ints = new int[] { 1, 5, 7, 3, 2, 3 };
			for (int i = 0; i < ints.Length; i++)
			{
				Db().Store(new IndexedQueriesTestCase.IndexedQueriesItem(ints[i]));
			}
		}

		public virtual void TestIntQuery()
		{
			AssertInts(5);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStringQuery()
		{
			AssertNullNameCount(6);
			Db().Store(new IndexedQueriesTestCase.IndexedQueriesItem("d"));
			AssertQuery(1, "b");
			UpdateB();
			Db().Store(new IndexedQueriesTestCase.IndexedQueriesItem("z"));
			Db().Store(new IndexedQueriesTestCase.IndexedQueriesItem("y"));
			Reopen();
			AssertQuery(1, "b");
			AssertInts(8);
		}

		private void AssertIntegers()
		{
			IQuery q = NewQuery();
			q.Descend("_integer").Constrain(4).Greater().Equal();
			AssertIntsFound(new int[] { 5, 7 }, q);
			q = NewQuery();
			q.Descend("_integer").Constrain(4).Smaller();
			AssertIntsFound(new int[] { 1, 2, 3, 3 }, q);
		}

		private void AssertInts(int expectedZeroSize)
		{
			IQuery q = NewQuery();
			q.Descend("_int").Constrain(0);
			int zeroSize = q.Execute().Count;
			Assert.AreEqual(expectedZeroSize, zeroSize);
			q = NewQuery();
			q.Descend("_int").Constrain(4).Greater().Equal();
			AssertIntsFound(new int[] { 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(4).Greater();
			AssertIntsFound(new int[] { 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(3).Greater();
			AssertIntsFound(new int[] { 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(3).Greater().Equal();
			AssertIntsFound(new int[] { 3, 3, 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(2).Greater().Equal();
			AssertIntsFound(new int[] { 2, 3, 3, 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(2).Greater();
			AssertIntsFound(new int[] { 3, 3, 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(1).Greater().Equal();
			AssertIntsFound(new int[] { 1, 2, 3, 3, 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(1).Greater();
			AssertIntsFound(new int[] { 2, 3, 3, 5, 7 }, q);
			q = NewQuery();
			q.Descend("_int").Constrain(4).Smaller();
			AssertIntsFound(new int[] { 1, 2, 3, 3 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(4).Smaller().Equal();
			AssertIntsFound(new int[] { 1, 2, 3, 3 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(3).Smaller();
			AssertIntsFound(new int[] { 1, 2 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(3).Smaller().Equal();
			AssertIntsFound(new int[] { 1, 2, 3, 3 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(2).Smaller().Equal();
			AssertIntsFound(new int[] { 1, 2 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(2).Smaller();
			AssertIntsFound(new int[] { 1 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(1).Smaller().Equal();
			AssertIntsFound(new int[] { 1 }, expectedZeroSize, q);
			q = NewQuery();
			q.Descend("_int").Constrain(1).Smaller();
			AssertIntsFound(new int[] {  }, expectedZeroSize, q);
		}

		private void AssertIntsFound(int[] ints, int zeroSize, IQuery q)
		{
			IObjectSet res = q.Execute();
			Assert.AreEqual((ints.Length + zeroSize), res.Count);
			while (res.HasNext())
			{
				IndexedQueriesTestCase.IndexedQueriesItem ci = (IndexedQueriesTestCase.IndexedQueriesItem
					)res.Next();
				for (int i = 0; i < ints.Length; i++)
				{
					if (ints[i] == ci._int)
					{
						ints[i] = 0;
						break;
					}
				}
			}
			for (int i = 0; i < ints.Length; i++)
			{
				Assert.AreEqual(0, ints[i]);
			}
		}

		private void AssertIntsFound(int[] ints, IQuery q)
		{
			AssertIntsFound(ints, 0, q);
		}

		private void AssertQuery(int count, string @string)
		{
			IObjectSet res = QueryForName(@string);
			Assert.AreEqual(count, res.Count);
			IndexedQueriesTestCase.IndexedQueriesItem item = (IndexedQueriesTestCase.IndexedQueriesItem
				)res.Next();
			Assert.AreEqual("b", item._name);
		}

		private void AssertNullNameCount(int count)
		{
			IObjectSet res = QueryForName(null);
			Assert.AreEqual(count, res.Count);
			while (res.HasNext())
			{
				IndexedQueriesTestCase.IndexedQueriesItem ci = (IndexedQueriesTestCase.IndexedQueriesItem
					)res.Next();
				Assert.IsNull(ci._name);
			}
		}

		private void UpdateB()
		{
			IObjectSet res = QueryForName("b");
			IndexedQueriesTestCase.IndexedQueriesItem ci = (IndexedQueriesTestCase.IndexedQueriesItem
				)res.Next();
			ci._name = "j";
			Db().Store(ci);
			res = QueryForName("b");
			Assert.AreEqual(0, res.Count);
			res = QueryForName("j");
			Assert.AreEqual(1, res.Count);
			ci._name = "b";
			Db().Store(ci);
			AssertQuery(1, "b");
		}

		private IObjectSet QueryForName(string n)
		{
			IQuery q = NewQuery();
			q.Descend("_name").Constrain(n);
			return q.Execute();
		}

		protected override IQuery NewQuery()
		{
			IQuery q = base.NewQuery();
			q.Constrain(typeof(IndexedQueriesTestCase.IndexedQueriesItem));
			return q;
		}
	}
}
