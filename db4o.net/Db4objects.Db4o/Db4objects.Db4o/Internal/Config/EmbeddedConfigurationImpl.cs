/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.Internal.Config
{
	public class EmbeddedConfigurationImpl : IEmbeddedConfiguration, ILegacyConfigurationProvider
	{
		private readonly Config4Impl _legacy;

		private IList _configItems;

		public EmbeddedConfigurationImpl(IConfiguration legacy)
		{
			_legacy = (Config4Impl)legacy;
		}

		public virtual ICacheConfiguration Cache
		{
			get
			{
				return new CacheConfigurationImpl(_legacy);
			}
		}

		public virtual IFileConfiguration File
		{
			get
			{
				return new FileConfigurationImpl(_legacy);
			}
		}

		public virtual ICommonConfiguration Common
		{
			get
			{
				return Db4oLegacyConfigurationBridge.AsCommonConfiguration(Legacy());
			}
		}

		public virtual Config4Impl Legacy()
		{
			return _legacy;
		}

		public virtual void AddConfigurationItem(IEmbeddedConfigurationItem configItem)
		{
			if (_configItems != null && _configItems.Contains(configItem))
			{
				return;
			}
			configItem.Prepare(this);
			if (_configItems == null)
			{
				_configItems = new ArrayList();
			}
			_configItems.Add(configItem);
		}

		public virtual void ApplyConfigurationItems(IEmbeddedObjectContainer container)
		{
			if (_configItems == null)
			{
				return;
			}
			for (IEnumerator configItemIter = _configItems.GetEnumerator(); configItemIter.MoveNext
				(); )
			{
				IEmbeddedConfigurationItem configItem = ((IEmbeddedConfigurationItem)configItemIter
					.Current);
				configItem.Apply(container);
			}
		}

		public virtual IIdSystemConfiguration IdSystem
		{
			get
			{
				return new IdSystemConfigurationImpl(_legacy);
			}
		}
	}
}
