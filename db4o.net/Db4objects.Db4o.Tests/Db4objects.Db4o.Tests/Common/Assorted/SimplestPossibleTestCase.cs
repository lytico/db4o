/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class SimplestPossibleTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new SimplestPossibleTestCase().RunNetworking();
		}

		protected override void Store()
		{
			Db().Store(new SimplestPossibleTestCase.Item("one"));
		}

		public virtual void Test()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(SimplestPossibleTestCase.Item));
			q.Descend("name").Constrain("one");
			IObjectSet objectSet = q.Execute();
			SimplestPossibleTestCase.Item item = (SimplestPossibleTestCase.Item)objectSet.Next
				();
			Assert.IsNotNull(item);
			Assert.AreEqual("one", item.GetName());
		}

		public class Item
		{
			public string name;

			public Item()
			{
			}

			public Item(string name_)
			{
				this.name = name_;
			}

			public virtual string GetName()
			{
				return name;
			}
		}
	}
}
