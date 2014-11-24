/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class DeactivateTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new DeactivateTestCase.Item("foo", new DeactivateTestCase.Item("bar", 
				null)));
		}

		public virtual void Test()
		{
			IQuery query = NewQuery();
			query.Descend("_name").Constrain("foo");
			IObjectSet results = query.Execute();
			Assert.AreEqual(1, results.Count);
			DeactivateTestCase.Item item1 = (DeactivateTestCase.Item)results.Next();
			DeactivateTestCase.Item item2 = item1._child;
			Assert.IsTrue(Db().IsActive(item1));
			Assert.IsTrue(Db().IsActive(item2));
			Db().Deactivate(item1);
			Assert.IsFalse(Db().IsActive(item1));
			Assert.IsTrue(Db().IsActive(item2));
		}

		public static void Main(string[] args)
		{
			new DeactivateTestCase().RunAll();
		}

		public class Item
		{
			public DeactivateTestCase.Item _child;

			public string _name;

			public Item(string name, DeactivateTestCase.Item child)
			{
				_name = name;
				_child = child;
			}
		}
	}
}
