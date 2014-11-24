/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class EmbeddedConfigurationItemIntegrationTestCase : ITestCase
	{
		public virtual void Test()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = new MemoryStorage();
			EmbeddedConfigurationItemIntegrationTestCase.DummyConfigurationItem item = new EmbeddedConfigurationItemIntegrationTestCase.DummyConfigurationItem
				(this);
			config.AddConfigurationItem(item);
			IEmbeddedObjectContainer container = Db4oEmbedded.OpenFile(config, string.Empty);
			item.Verify(config, container);
			container.Close();
		}

		private sealed class DummyConfigurationItem : IEmbeddedConfigurationItem
		{
			private int _prepareCount = 0;

			private int _applyCount = 0;

			private IEmbeddedConfiguration _config;

			private IEmbeddedObjectContainer _container;

			public void Prepare(IEmbeddedConfiguration configuration)
			{
				this._config = configuration;
				this._prepareCount++;
			}

			public void Apply(IEmbeddedObjectContainer container)
			{
				this._container = container;
				this._applyCount++;
			}

			internal void Verify(IEmbeddedConfiguration config, IEmbeddedObjectContainer container
				)
			{
				Assert.AreSame(config, this._config);
				Assert.AreSame(container, this._container);
				Assert.AreEqual(1, this._prepareCount);
				Assert.AreEqual(1, this._applyCount);
			}

			internal DummyConfigurationItem(EmbeddedConfigurationItemIntegrationTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			private readonly EmbeddedConfigurationItemIntegrationTestCase _enclosing;
		}
	}
}
