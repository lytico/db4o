/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class MemoryBinIsReusableTestCase : ITestCase
	{
		private static readonly string ItemName = "foo";

		private static readonly string BinUri = "mybin";

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		public virtual void Test()
		{
			MemoryStorage origStorage = new MemoryStorage();
			IEmbeddedConfiguration origConfig = Config(origStorage);
			IObjectContainer origDb = Db4oEmbedded.OpenFile(origConfig, BinUri);
			origDb.Store(new MemoryBinIsReusableTestCase.Item(ItemName));
			origDb.Close();
			MemoryBin origBin = origStorage.Bin(BinUri);
			byte[] data = origBin.Data();
			Assert.AreEqual(data.Length, origBin.Length());
			MemoryBin newBin = new MemoryBin(data, new DoublingGrowthStrategy());
			MemoryStorage newStorage = new MemoryStorage();
			newStorage.Bin(BinUri, newBin);
			IObjectContainer newDb = Db4oEmbedded.OpenFile(Config(newStorage), BinUri);
			IObjectSet result = newDb.Query(typeof(MemoryBinIsReusableTestCase.Item));
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(ItemName, ((MemoryBinIsReusableTestCase.Item)result.Next())._name
				);
			newDb.Close();
		}

		private IEmbeddedConfiguration Config(MemoryStorage storage)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = storage;
			config.Common.ReflectWith(Platform4.ReflectorForType(typeof(MemoryBinIsReusableTestCase.Item
				)));
			return config;
		}
	}
}
