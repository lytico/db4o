/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

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
	public class SimpleListQueryTestCase : AbstractDb4oTestCase
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

			public override bool Equals(object obj)
			{
				if (!(obj is SimpleListQueryTestCase.ReferenceTypeElement))
				{
					return false;
				}
				SimpleListQueryTestCase.ReferenceTypeElement other = (SimpleListQueryTestCase.ReferenceTypeElement
					)obj;
				if (name == null)
				{
					return other.name == null;
				}
				return name.Equals(other.name);
			}
		}

		internal static readonly object[] Data = new object[] { "one", "two", 1, 2, 42, new 
			SimpleListQueryTestCase.ReferenceTypeElement("one"), new SimpleListQueryTestCase.ReferenceTypeElement
			("fortytwo") };

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(SimpleListQueryTestCase.Item)).CascadeOnDelete(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < Data.Length; i++)
			{
				StoreItem(Data[i]);
			}
		}

		private void StoreItem(object listElement)
		{
			SimpleListQueryTestCase.Item item = new SimpleListQueryTestCase.Item();
			item.list = new ArrayList();
			item.list.Add(listElement);
			Store(item);
		}

		public virtual void TestListConstrainQuery()
		{
			for (int i = 0; i < Data.Length; i++)
			{
				AssertSingleElementQuery(Data[i]);
			}
		}

		private void AssertSingleElementQuery(object element)
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(SimpleListQueryTestCase.Item));
			q.Descend("list").Constrain(element);
			AssertSingleElementQueryResult(q, element);
		}

		private void AssertSingleElementQueryResult(IQuery query, object element)
		{
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(1, objectSet.Count);
			SimpleListQueryTestCase.Item item = (SimpleListQueryTestCase.Item)objectSet.Next(
				);
			Assert.AreEqual(element, item.list[0]);
		}
	}
}
