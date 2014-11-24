/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Uuid;

namespace Db4objects.Db4o.Tests.Common.Uuid
{
	public class DeleteUUIDTestCase : AbstractDb4oTestCase
	{
		private Db4oUUID _uuid;

		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			DeleteUUIDTestCase.Item item = new DeleteUUIDTestCase.Item();
			Store(item);
			_uuid = Db().GetObjectInfo(item).GetUUID();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDelete()
		{
			DeleteUUIDTestCase.Item item = ((DeleteUUIDTestCase.Item)RetrieveOnlyInstance(typeof(
				DeleteUUIDTestCase.Item)));
			Db().Delete(item);
			Assert.IsNull(Db().GetByUUID(_uuid));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeleteCommit()
		{
			DeleteUUIDTestCase.Item item = ((DeleteUUIDTestCase.Item)RetrieveOnlyInstance(typeof(
				DeleteUUIDTestCase.Item)));
			Db().Delete(item);
			Db().Commit();
			Assert.IsNull(Db().GetByUUID(_uuid));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeleteRollback()
		{
			DeleteUUIDTestCase.Item item = ((DeleteUUIDTestCase.Item)RetrieveOnlyInstance(typeof(
				DeleteUUIDTestCase.Item)));
			Db().Delete(item);
			Db().Rollback();
			Assert.IsNotNull(Db().GetByUUID(_uuid));
		}
	}
}
