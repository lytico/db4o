/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Freespace;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class BlockConfigurationFileSizeTestCase : FileSizeTestCaseBase
	{
		public static void Main(string[] args)
		{
			new BlockConfigurationFileSizeTestCase().RunSolo();
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.BlockSize(8);
		}

		public virtual void Test()
		{
			Store(new BlockConfigurationFileSizeTestCase.Item("one"));
			Db().Commit();
			int initialSize = DatabaseFileSize();
			for (int i = 0; i < 100; i++)
			{
				Store(new BlockConfigurationFileSizeTestCase.Item("two"));
			}
			Db().Commit();
			int modifiedSize = DatabaseFileSize();
			int sizeIncrease = modifiedSize - initialSize;
			Assert.IsSmaller(30000, sizeIncrease);
		}
	}
}
