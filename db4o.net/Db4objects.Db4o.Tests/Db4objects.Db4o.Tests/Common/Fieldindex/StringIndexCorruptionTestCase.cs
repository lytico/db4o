/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	/// <summary>Jira ticket: COR-373</summary>
	/// <exclude></exclude>
	public class StringIndexCorruptionTestCase : StringIndexTestCaseBase
	{
		public static void Main(string[] arguments)
		{
			new StringIndexCorruptionTestCase().RunSolo();
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.BTreeNodeSize(4);
		}

		public virtual void TestStressSet()
		{
			IExtObjectContainer container = Db();
			int itemCount = 300;
			for (int i = 0; i < itemCount; ++i)
			{
				StringIndexTestCaseBase.Item item = new StringIndexTestCaseBase.Item(ItemName(i));
				container.Store(item);
				container.Store(item);
				container.Commit();
				container.Store(item);
				container.Store(item);
				container.Commit();
			}
			for (int i = 0; i < itemCount; ++i)
			{
				string itemName = ItemName(i);
				StringIndexTestCaseBase.Item found = Query(itemName);
				Assert.IsNotNull(found, "'" + itemName + "' not found");
				Assert.AreEqual(itemName, found.name);
			}
		}

		private string ItemName(int i)
		{
			return "item " + i;
		}
	}
}
