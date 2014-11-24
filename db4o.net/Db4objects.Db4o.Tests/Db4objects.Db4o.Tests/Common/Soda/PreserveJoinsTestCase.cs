/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class PreserveJoinsTestCase : AbstractDb4oTestCase
	{
		public class Parent
		{
			public Parent(PreserveJoinsTestCase.Child child, string value)
			{
				this.child = child;
				this.value = value;
			}

			public PreserveJoinsTestCase.Child child;

			public string value;
		}

		public class Child
		{
			public Child(string name)
			{
				this.name = name;
			}

			public string name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new PreserveJoinsTestCase.Parent(new PreserveJoinsTestCase.Child("bar"), "parent"
				));
		}

		public virtual void Test()
		{
			IQuery barQuery = Db().Query();
			barQuery.Constrain(typeof(PreserveJoinsTestCase.Child));
			barQuery.Descend("name").Constrain("bar");
			object barObj = barQuery.Execute().Next();
			IQuery query = Db().Query();
			query.Constrain(typeof(PreserveJoinsTestCase.Parent));
			IConstraint c1 = query.Descend("value").Constrain("dontexist");
			IConstraint c2 = query.Descend("child").Constrain(barObj);
			IConstraint c1_and_c2 = c1.And(c2);
			IConstraint cParent = query.Descend("value").Constrain("parent");
			c1_and_c2.Or(cParent);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
