/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class PlainObjectTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new PlainObjectTestCase().RunAll();
		}

		public class Item
		{
			public string _name;

			public object _plainObject;

			public Item(string name, object plainObject)
			{
				_name = name;
				_plainObject = plainObject;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(PlainObjectTestCase.Item)).CascadeOnDelete(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			object plainObject = new object();
			PlainObjectTestCase.Item item = new PlainObjectTestCase.Item("one", plainObject);
			Store(item);
			Assert.IsTrue(Db().IsStored(item._plainObject));
			Store(new PlainObjectTestCase.Item("two", plainObject));
		}

		public virtual void TestRetrieve()
		{
			PlainObjectTestCase.Item itemOne = RetrieveItem("one");
			Assert.IsNotNull(itemOne._plainObject);
			Assert.IsTrue(Db().IsStored(itemOne._plainObject));
			PlainObjectTestCase.Item itemTwo = RetrieveItem("two");
			Assert.AreSame(itemOne._plainObject, itemTwo._plainObject);
		}

		public virtual void TestDelete()
		{
			PlainObjectTestCase.Item itemOne = RetrieveItem("one");
			Db().Delete(itemOne);
		}

		public virtual void _testEvaluationQuery()
		{
			// The evaluation doesn't work in C/S mode
			// because TransportObjectContainer#storeInteral  
			// never gets a chance to intercept.
			PlainObjectTestCase.Item itemOne = RetrieveItem("one");
			object plainObject = itemOne._plainObject;
			IQuery q = NewQuery(typeof(PlainObjectTestCase.Item));
			q.Constrain(new _IEvaluation_64(plainObject));
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(2, objectSet.Count);
		}

		private sealed class _IEvaluation_64 : IEvaluation
		{
			public _IEvaluation_64(object plainObject)
			{
				this.plainObject = plainObject;
			}

			public void Evaluate(ICandidate candidate)
			{
				PlainObjectTestCase.Item item = (PlainObjectTestCase.Item)candidate.GetObject();
				candidate.Include(item._plainObject == plainObject);
			}

			private readonly object plainObject;
		}

		public virtual void TestIdentityQuery()
		{
			PlainObjectTestCase.Item itemOne = RetrieveItem("one");
			object plainObject = itemOne._plainObject;
			IQuery q = NewQuery(typeof(PlainObjectTestCase.Item));
			q.Descend("_plainObject").Constrain(plainObject).Identity();
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(2, objectSet.Count);
		}

		private PlainObjectTestCase.Item RetrieveItem(string name)
		{
			IQuery query = NewQuery(typeof(PlainObjectTestCase.Item));
			query.Descend("_name").Constrain(name);
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(1, objectSet.Count);
			return (PlainObjectTestCase.Item)objectSet.Next();
		}
	}
}
