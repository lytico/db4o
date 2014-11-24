/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class TwoLevelIndexTestCase : AbstractDb4oTestCase
	{
		public class Parent1
		{
			public TwoLevelIndexTestCase.Child1 child;
		}

		public class Parent2 : TwoLevelIndexTestCase.Parent1
		{
		}

		public class Child1
		{
			public int id;
		}

		public class Child2 : TwoLevelIndexTestCase.Child1
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(TwoLevelIndexTestCase.Parent1)).ObjectField("child").Indexed
				(true);
			config.ObjectClass(typeof(TwoLevelIndexTestCase.Child1)).ObjectField("id").Indexed
				(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			TwoLevelIndexTestCase.Parent1 parent1 = new TwoLevelIndexTestCase.Parent1();
			parent1.child = new TwoLevelIndexTestCase.Child1();
			parent1.child.id = 42;
			Store(parent1);
			TwoLevelIndexTestCase.Parent2 parent2 = new TwoLevelIndexTestCase.Parent2();
			parent2.child = new TwoLevelIndexTestCase.Child2();
			parent2.child.id = 42;
			Store(parent2);
		}

		public virtual void TestTwoLevelParentIsSubclassed()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(TwoLevelIndexTestCase.Parent2));
			query.Descend("child").Descend("id").Constrain(42);
			Assert.AreEqual(1, query.Execute().Count);
		}

		public virtual void TestChildClassConstrained()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(TwoLevelIndexTestCase.Parent1));
			query.Descend("child").Descend("id").Constrain(42);
			query.Descend("child").Constrain(typeof(TwoLevelIndexTestCase.Child2));
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
