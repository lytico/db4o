/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class EncryptedFileMigrationTestCase : HandlerUpdateTestCaseBase
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
		}

		// do nothing
		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			EncryptedFileMigrationTestCase.Item item = (EncryptedFileMigrationTestCase.Item)values
				[0];
			Assert.AreEqual("one", item._name);
		}

		protected override object CreateArrays()
		{
			return null;
		}

		protected override object[] CreateValues()
		{
			return new object[] { new EncryptedFileMigrationTestCase.Item("one") };
		}

		protected override string TypeName()
		{
			return "encrypted";
		}

		protected override void ConfigureForStore(IConfiguration config)
		{
			ConfigureInternal(config);
		}

		protected override void ConfigureForTest(IConfiguration config)
		{
			ConfigureInternal(config);
		}

		private void ConfigureInternal(IConfiguration config)
		{
			config.Encrypt(true);
			config.Password("encrypted");
		}

		protected override void DeconfigureForStore(IConfiguration config)
		{
			DeconfigureInternal(config);
		}

		protected override void DeconfigureForTest(IConfiguration config)
		{
			DeconfigureInternal(config);
		}

		private void DeconfigureInternal(IConfiguration config)
		{
			config.Encrypt(false);
		}
	}
}
