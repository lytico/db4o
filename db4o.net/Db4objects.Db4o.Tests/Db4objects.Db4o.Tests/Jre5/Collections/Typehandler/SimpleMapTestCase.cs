/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	/// <exclude></exclude>
	public class SimpleMapTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public IDictionary map;
		}

		public class ReferenceTypeElement
		{
			public string name;

			public ReferenceTypeElement(string name_)
			{
				name = name_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(Hashtable))
				, new MapTypeHandler());
			config.ObjectClass(typeof(SimpleMapTestCase.Item)).CascadeOnDelete(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			SimpleMapTestCase.Item item = new SimpleMapTestCase.Item();
			item.map = new Hashtable();
			item.map["zero"] = "zero";
			item.map[new SimpleMapTestCase.ReferenceTypeElement("one")] = "one";
			Store(item);
		}

		public virtual void TestRetrieveInstance()
		{
			SimpleMapTestCase.Item item = (SimpleMapTestCase.Item)RetrieveOnlyInstance(typeof(
				SimpleMapTestCase.Item));
			Assert.AreEqual("zero", item.map["zero"]);
		}

		public virtual void TestQuery()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(SimpleMapTestCase.Item));
			q.Descend("map").Constrain("zero");
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(1, objectSet.Count);
			SimpleMapTestCase.Item item = (SimpleMapTestCase.Item)objectSet.Next();
			Assert.AreEqual("zero", item.map["zero"]);
		}

		public virtual void TestDeletion()
		{
			AssertObjectCount(typeof(SimpleMapTestCase.ReferenceTypeElement), 1);
			SimpleMapTestCase.Item item = (SimpleMapTestCase.Item)RetrieveOnlyInstance(typeof(
				SimpleMapTestCase.Item));
			Db().Delete(item);
			AssertObjectCount(typeof(SimpleMapTestCase.ReferenceTypeElement), 0);
		}

		private void AssertObjectCount(Type clazz, int count)
		{
			Assert.AreEqual(count, Db().Query(clazz).Count);
		}
	}
}
