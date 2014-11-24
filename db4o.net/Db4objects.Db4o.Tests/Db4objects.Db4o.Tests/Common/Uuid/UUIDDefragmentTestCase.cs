/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Uuid;

namespace Db4objects.Db4o.Tests.Common.Uuid
{
	/// <exclude></exclude>
	public class UUIDDefragmentTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			UUIDDefragmentTestCase.Item item = new UUIDDefragmentTestCase.Item();
			item.name = "one";
			Store(item);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Db4oUUID uuidBeforeDefragment = SingleItemUUID();
			byte[] signatureBeforeDefragment = uuidBeforeDefragment.GetSignaturePart();
			long longPartBeforeDefragment = uuidBeforeDefragment.GetLongPart();
			Defragment();
			Db4oUUID uuidAfterDefragment = SingleItemUUID();
			byte[] signatureAfterDefragment = uuidAfterDefragment.GetSignaturePart();
			long longPartAfterDefragment = uuidAfterDefragment.GetLongPart();
			ArrayAssert.AreEqual(signatureBeforeDefragment, signatureAfterDefragment);
			Assert.AreEqual(longPartBeforeDefragment, longPartAfterDefragment);
		}

		private Db4oUUID SingleItemUUID()
		{
			UUIDDefragmentTestCase.Item item = (UUIDDefragmentTestCase.Item)((UUIDDefragmentTestCase.Item
				)RetrieveOnlyInstance(typeof(UUIDDefragmentTestCase.Item)));
			IObjectInfo objectInfo = Db().GetObjectInfo(item);
			Db4oUUID uuid = objectInfo.GetUUID();
			return uuid;
		}
	}
}
