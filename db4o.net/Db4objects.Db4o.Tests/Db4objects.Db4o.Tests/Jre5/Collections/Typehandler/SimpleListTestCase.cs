/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	/// <exclude></exclude>
	public class SimpleListTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public IList list;
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
			config.ObjectClass(typeof(SimpleListTestCase.Item)).CascadeOnDelete(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			SimpleListTestCase.Item item = new SimpleListTestCase.Item();
			item.list = new ArrayList();
			item.list.Add("zero");
			item.list.Add(new SimpleListTestCase.ReferenceTypeElement("one"));
			Store(item);
		}

		public virtual void TestRetrieveInstance()
		{
			SimpleListTestCase.Item item = (SimpleListTestCase.Item)RetrieveOnlyInstance(typeof(
				SimpleListTestCase.Item));
			Assert.AreEqual(2, item.list.Count);
			Assert.AreEqual("zero", item.list[0]);
		}

		public virtual void TestCascadingActivation()
		{
			SimpleListTestCase.Item item = (SimpleListTestCase.Item)RetrieveOnlyInstance(typeof(
				SimpleListTestCase.Item));
			IList list = item.list;
			Assert.AreEqual(2, list.Count);
			object element = list[1];
			if (Db().IsActive(element))
			{
				Db().Deactivate(item, int.MaxValue);
				Assert.IsFalse(Db().IsActive(element));
				Db().Activate(item, int.MaxValue);
				Assert.IsTrue(Db().IsActive(element));
			}
		}

		public virtual void TestQuery()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(SimpleListTestCase.Item));
			q.Descend("list").Constrain("zero");
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(1, objectSet.Count);
			SimpleListTestCase.Item item = (SimpleListTestCase.Item)objectSet.Next();
			Assert.AreEqual("zero", item.list[0]);
		}

		public virtual void TestDeletion()
		{
			AssertObjectCount(typeof(SimpleListTestCase.ReferenceTypeElement), 1);
			SimpleListTestCase.Item item = (SimpleListTestCase.Item)RetrieveOnlyInstance(typeof(
				SimpleListTestCase.Item));
			Db().Delete(item);
			AssertObjectCount(typeof(SimpleListTestCase.ReferenceTypeElement), 0);
		}

		private void AssertObjectCount(Type clazz, int count)
		{
			Assert.AreEqual(count, Db().Query(clazz).Count);
		}
	}
}
