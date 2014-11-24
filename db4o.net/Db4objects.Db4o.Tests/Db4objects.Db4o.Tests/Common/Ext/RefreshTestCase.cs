/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class RefreshTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new RefreshTestCase().RunAll();
		}

		public class Item
		{
			public string name;

			public RefreshTestCase.Item child;

			public Item(string name, RefreshTestCase.Item child)
			{
				this.name = name;
				this.child = child;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(RefreshTestCase.Item)).CascadeOnUpdate(true);
		}

		protected override void Store()
		{
			RefreshTestCase.Item r3 = new RefreshTestCase.Item("o3", null);
			RefreshTestCase.Item r2 = new RefreshTestCase.Item("o2", r3);
			RefreshTestCase.Item r1 = new RefreshTestCase.Item("o1", r2);
			Store(r1);
		}

		public virtual void Test()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			try
			{
				RefreshTestCase.Item r1 = GetRoot(oc1);
				r1.name = "cc";
				oc1.Refresh(r1, 0);
				Assert.AreEqual("cc", r1.name);
				oc1.Refresh(r1, 1);
				Assert.AreEqual("o1", r1.name);
				r1.child.name = "cc";
				oc1.Refresh(r1, 1);
				Assert.AreEqual("cc", r1.child.name);
				oc1.Refresh(r1, 2);
				Assert.AreEqual("o2", r1.child.name);
				RefreshTestCase.Item r2 = GetRoot(oc2);
				r2.name = "o21";
				r2.child.name = "o22";
				r2.child.child.name = "o23";
				oc2.Store(r2);
				oc2.Commit();
				oc1.Refresh(r1, 3);
				Assert.AreEqual("o21", r1.name);
				Assert.AreEqual("o22", r1.child.name);
				Assert.AreEqual("o23", r1.child.child.name);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
			}
		}

		private RefreshTestCase.Item GetRoot(IObjectContainer oc)
		{
			return GetByName(oc, "o1");
		}

		private RefreshTestCase.Item GetByName(IObjectContainer oc, string name)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(RefreshTestCase.Item));
			q.Descend("name").Constrain(name);
			IObjectSet objectSet = q.Execute();
			return (RefreshTestCase.Item)objectSet.Next();
		}
	}
}
