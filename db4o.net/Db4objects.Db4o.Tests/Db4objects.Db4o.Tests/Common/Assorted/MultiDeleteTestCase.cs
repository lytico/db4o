/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class MultiDeleteTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		public static void Main(string[] args)
		{
			new MultiDeleteTestCase().RunSoloAndClientServer();
		}

		public class Item
		{
			public MultiDeleteTestCase.Item child;

			public string name;

			public object forLong;

			public long myLong;

			public object[] untypedArr;

			public long[] typedArr;

			public virtual void SetMembers()
			{
				forLong = System.Convert.ToInt64(100);
				myLong = System.Convert.ToInt64(100);
				untypedArr = new object[] { System.Convert.ToInt64(10), "hi", new MultiDeleteTestCase.Item
					() };
				typedArr = new long[] { System.Convert.ToInt64(3), System.Convert.ToInt64(7), System.Convert.ToInt64
					(9) };
			}
		}

		protected override void Configure(IConfiguration config)
		{
			IObjectClass itemClass = config.ObjectClass(typeof(MultiDeleteTestCase.Item));
			itemClass.CascadeOnDelete(true);
			itemClass.CascadeOnUpdate(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			MultiDeleteTestCase.Item md = new MultiDeleteTestCase.Item();
			md.name = "killmefirst";
			md.SetMembers();
			md.child = new MultiDeleteTestCase.Item();
			md.child.SetMembers();
			Db().Store(md);
		}

		public virtual void TestDeleteCanBeCalledTwice()
		{
			MultiDeleteTestCase.Item item = ItemByName("killmefirst");
			Assert.IsNotNull(item);
			long id = Db().GetID(item);
			Db().Delete(item);
			Assert.AreSame(item, ItemById(id));
			Db().Delete(item);
			Assert.AreSame(item, ItemById(id));
		}

		private MultiDeleteTestCase.Item ItemByName(string name)
		{
			IQuery q = NewQuery(typeof(MultiDeleteTestCase.Item));
			q.Descend("name").Constrain(name);
			return (MultiDeleteTestCase.Item)q.Execute().Next();
		}

		private MultiDeleteTestCase.Item ItemById(long id)
		{
			return (MultiDeleteTestCase.Item)Db().GetByID(id);
		}
	}
}
