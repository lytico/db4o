/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class CsDeleteReaddTestCase : Db4oClientServerTestCase
	{
		public class ItemParent
		{
		}

		public class Item : CsDeleteReaddTestCase.ItemParent
		{
			public string name;

			public Item(string name_)
			{
				name = name_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
			config.GenerateCommitTimestamps(true);
			config.ObjectClass(typeof(CsDeleteReaddTestCase.Item)).ObjectField("name").Indexed
				(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new CsDeleteReaddTestCase.Item("one"));
		}

		public virtual void TestDeleteReadd()
		{
			IExtObjectContainer client1 = Db();
			IExtObjectContainer client2 = OpenNewSession();
			CsDeleteReaddTestCase.Item item1 = (CsDeleteReaddTestCase.Item)((CsDeleteReaddTestCase.Item
				)RetrieveOnlyInstance(client1, typeof(CsDeleteReaddTestCase.Item)));
			CsDeleteReaddTestCase.Item item2 = (CsDeleteReaddTestCase.Item)((CsDeleteReaddTestCase.Item
				)RetrieveOnlyInstance(client2, typeof(CsDeleteReaddTestCase.Item)));
			long idBeforeDelete = client1.GetID(item1);
			client1.Delete(item1);
			client1.Commit();
			client2.Store(item2);
			client2.Commit();
			client2.Close();
			CsDeleteReaddTestCase.Item item3 = ((CsDeleteReaddTestCase.Item)RetrieveOnlyInstance
				(client1, typeof(CsDeleteReaddTestCase.Item)));
			long idAfterUpdate = client1.GetID(item3);
			Assert.AreEqual(idBeforeDelete, idAfterUpdate);
			new FieldIndexAssert(typeof(CsDeleteReaddTestCase.Item), "name").AssertSingleEntry
				(FileSession(), idAfterUpdate);
		}

		public static void Main(string[] arguments)
		{
			new CsDeleteReaddTestCase().RunAll();
		}
	}
}
#endif // !SILVERLIGHT
