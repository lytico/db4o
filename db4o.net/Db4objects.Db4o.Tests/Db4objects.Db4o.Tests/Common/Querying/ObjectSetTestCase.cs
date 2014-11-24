/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	/// <exclude></exclude>
	public class ObjectSetTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ObjectSetTestCase().RunNetworking();
		}

		public class Item
		{
			public string name;

			public Item()
			{
			}

			public Item(string name_)
			{
				name = name_;
			}

			public override string ToString()
			{
				return "Item(\"" + name + "\")";
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Queries().EvaluationMode(QueryEvaluationMode.Lazy);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new ObjectSetTestCase.Item("foo"));
			Db().Store(new ObjectSetTestCase.Item("bar"));
			Db().Store(new ObjectSetTestCase.Item("baz"));
		}

		public virtual void _testObjectsCantBeSeenAfterDelete()
		{
			Transaction trans1 = NewTransaction();
			Transaction trans2 = NewTransaction();
			DeleteItemAndCommit(trans2, "foo");
			IObjectSet os = QueryItems(trans1);
			AssertItems(new string[] { "bar", "baz" }, os);
		}

		public virtual void _testAccessOrder()
		{
			IObjectSet result = NewQuery(typeof(ObjectSetTestCase.Item)).Execute();
			for (int i = 0; i < result.Count; ++i)
			{
				Assert.IsTrue(result.HasNext());
				Assert.AreSame(result.Ext()[i], result.Next());
			}
			Assert.IsFalse(result.HasNext());
		}

		public virtual void TestInvalidNext()
		{
			IQuery query = NewQuery(typeof(ObjectSetTestCase.Item));
			query.Descend("name").Constrain("foo");
			IObjectSet result = query.Execute();
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_69(result));
		}

		private sealed class _ICodeBlock_69 : ICodeBlock
		{
			public _ICodeBlock_69(IObjectSet result)
			{
				this.result = result;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				while (true)
				{
					result.HasNext();
					result.Next();
				}
			}

			private readonly IObjectSet result;
		}

		private void AssertItems(string[] expectedNames, IObjectSet actual)
		{
			for (int i = 0; i < expectedNames.Length; i++)
			{
				Assert.IsTrue(actual.HasNext());
				Assert.AreEqual(expectedNames[i], ((ObjectSetTestCase.Item)actual.Next()).name);
			}
			Assert.IsFalse(actual.HasNext());
		}

		private void DeleteItemAndCommit(Transaction trans, string name)
		{
			Container().Delete(trans, QueryItem(trans, name));
			trans.Commit();
		}

		private ObjectSetTestCase.Item QueryItem(Transaction trans, string name)
		{
			IQuery q = NewQuery(trans, typeof(ObjectSetTestCase.Item));
			q.Descend("name").Constrain(name);
			return (ObjectSetTestCase.Item)q.Execute().Next();
		}

		private IObjectSet QueryItems(Transaction trans)
		{
			IQuery q = NewQuery(trans, typeof(ObjectSetTestCase.Item));
			q.Descend("name").OrderAscending();
			return q.Execute();
		}
	}
}
