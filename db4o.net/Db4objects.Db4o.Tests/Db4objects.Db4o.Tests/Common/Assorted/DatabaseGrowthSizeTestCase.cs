/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Fileheader;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class DatabaseGrowthSizeTestCase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutIdSystem
	{
		private const int Size = 10000;

		private static readonly int MaximumHeaderSize = HeaderSize();

		private const int Reserve = Const4.PointerLength * 3;

		public static void Main(string[] args)
		{
			new DatabaseGrowthSizeTestCase().RunSolo();
		}

		private static int HeaderSize()
		{
			NewFileHeaderBase fileHeader = FileHeader.NewCurrentFileHeader();
			FileHeaderVariablePart variablePart = fileHeader.CreateVariablePart(null);
			return fileHeader.Length() + variablePart.MarshalledLength() + FileHeader.TransactionPointerLength
				 + Reserve;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.DatabaseGrowthSize(Size);
			config.BlockSize(3);
			Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UsePointerBasedSystem
				();
		}

		public virtual void Test()
		{
			Assert.IsGreater(Size, FileSession().FileLength());
			Assert.IsSmaller(Size + MaximumHeaderSize, FileSession().FileLength());
			DatabaseGrowthSizeTestCase.Item item = DatabaseGrowthSizeTestCase.Item.NewItem(Size
				);
			Store(item);
			Assert.IsGreater(Size * 2, FileSession().FileLength());
			Assert.IsSmaller(Size * 2 + MaximumHeaderSize, FileSession().FileLength());
			object retrievedItem = ((DatabaseGrowthSizeTestCase.Item)RetrieveOnlyInstance(typeof(
				DatabaseGrowthSizeTestCase.Item)));
			Assert.AreSame(item, retrievedItem);
		}

		public class Item
		{
			public byte[] _payload;

			public Item()
			{
			}

			public static DatabaseGrowthSizeTestCase.Item NewItem(int payloadSize)
			{
				DatabaseGrowthSizeTestCase.Item item = new DatabaseGrowthSizeTestCase.Item();
				item._payload = new byte[payloadSize];
				return item;
			}
		}
	}
}
