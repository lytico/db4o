/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.CS.Internal.Config
{
	public class ServerConfigurationImpl : NetworkingConfigurationProviderImpl, IServerConfiguration
	{
		private IList _configItems;

		public ServerConfigurationImpl(Config4Impl config) : base(config)
		{
		}

		public virtual ICacheConfiguration Cache
		{
			get
			{
				return new CacheConfigurationImpl(Legacy());
			}
		}

		public virtual IFileConfiguration File
		{
			get
			{
				return Db4oLegacyConfigurationBridge.AsFileConfiguration(Legacy());
			}
		}

		public virtual ICommonConfiguration Common
		{
			get
			{
				return Db4oLegacyConfigurationBridge.AsCommonConfiguration(Legacy());
			}
		}

		public virtual int TimeoutServerSocket
		{
			get
			{
				return Legacy().TimeoutServerSocket();
			}
			set
			{
				int milliseconds = value;
				Legacy().TimeoutServerSocket(milliseconds);
			}
		}

		public virtual void AddConfigurationItem(IServerConfigurationItem configItem)
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

		public virtual void ApplyConfigurationItems(IObjectServer server)
		{
			if (_configItems == null)
			{
				return;
			}
			for (IEnumerator configItemIter = _configItems.GetEnumerator(); configItemIter.MoveNext
				(); )
			{
				IServerConfigurationItem configItem = ((IServerConfigurationItem)configItemIter.Current
					);
				configItem.Apply(server);
			}
		}

		public virtual IIdSystemConfiguration IdSystem
		{
			get
			{
				return new IdSystemConfigurationImpl(Legacy());
			}
		}
	}
}
