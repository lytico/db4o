/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class ObjectContainerCustomNameTestCase : ITestCase
	{
		private class CustomNameProvider : INameProvider
		{
			public virtual string Name(IObjectContainer db)
			{
				return CustomName;
			}
		}

		private static readonly string FileName = "foo.db4o";

		protected static readonly string CustomName = "custom";

		public virtual void TestDefault()
		{
			AssertName(Config(), FileName);
		}

		public virtual void TestCustom()
		{
			IEmbeddedConfiguration config = Config();
			config.Common.NameProvider(new ObjectContainerCustomNameTestCase.CustomNameProvider
				());
			AssertName(config, CustomName);
		}

		public virtual void TestNameIsAvailableAtConfigurationItemApplication()
		{
			IEmbeddedConfiguration config = Config();
			config.Common.NameProvider(new ObjectContainerCustomNameTestCase.CustomNameProvider
				());
			config.Common.Add(new _IConfigurationItem_35());
			AssertName(config, CustomName);
		}

		private sealed class _IConfigurationItem_35 : IConfigurationItem
		{
			public _IConfigurationItem_35()
			{
			}

			public void Apply(IInternalObjectContainer container)
			{
				Assert.AreEqual(ObjectContainerCustomNameTestCase.CustomName, container.ToString(
					));
			}

			public void Prepare(IConfiguration configuration)
			{
			}
		}

		private void AssertName(IEmbeddedConfiguration config, string expected)
		{
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(config, FileName);
			Assert.AreEqual(expected, db.ToString());
			db.Close();
		}

		private IEmbeddedConfiguration Config()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = new MemoryStorage();
			return config;
		}
	}
}
