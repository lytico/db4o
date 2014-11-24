/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class CascadeOnActivateTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new CascadeOnActivateTestCase().RunConcurrency();
		}

		public class Item
		{
			public string name;

			public CascadeOnActivateTestCase.Item child;
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(CascadeOnActivateTestCase.Item)).CascadeOnActivate(true
				);
		}

		protected override void Store()
		{
			CascadeOnActivateTestCase.Item item = new CascadeOnActivateTestCase.Item();
			item.name = "1";
			item.child = new CascadeOnActivateTestCase.Item();
			item.child.name = "2";
			item.child.child = new CascadeOnActivateTestCase.Item();
			item.child.child.name = "3";
			Store(item);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(CascadeOnActivateTestCase.Item));
			q.Descend("name").Constrain("1");
			IObjectSet os = q.Execute();
			CascadeOnActivateTestCase.Item item = (CascadeOnActivateTestCase.Item)os.Next();
			CascadeOnActivateTestCase.Item item3 = item.child.child;
			Assert.AreEqual("3", item3.name);
			oc.Deactivate(item, int.MaxValue);
			Assert.IsNull(item3.name);
			oc.Activate(item, 1);
			Assert.AreEqual("3", item3.name);
		}
	}
}
#endif // !SILVERLIGHT
