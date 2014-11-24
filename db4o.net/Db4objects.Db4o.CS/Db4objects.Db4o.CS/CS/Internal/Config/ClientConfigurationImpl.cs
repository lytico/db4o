/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.CS.Internal.Config
{
	public class ClientConfigurationImpl : NetworkingConfigurationProviderImpl, IClientConfiguration
	{
		private IList _configItems;

		public ClientConfigurationImpl(Config4Impl config) : base(config)
		{
		}

		public virtual IMessageSender MessageSender
		{
			get
			{
				return Legacy().GetMessageSender();
			}
		}

		public virtual int PrefetchIDCount
		{
			set
			{
				int prefetchIDCount = value;
				Legacy().PrefetchIDCount(prefetchIDCount);
			}
		}

		public virtual int PrefetchObjectCount
		{
			set
			{
				int prefetchObjectCount = value;
				Legacy().PrefetchObjectCount(prefetchObjectCount);
			}
		}

		public virtual ICommonConfiguration Common
		{
			get
			{
				return Db4oLegacyConfigurationBridge.AsCommonConfiguration(Legacy());
			}
		}

		public virtual int PrefetchDepth
		{
			set
			{
				int prefetchDepth = value;
				Legacy().PrefetchDepth(prefetchDepth);
			}
		}

		public virtual int PrefetchSlotCacheSize
		{
			set
			{
				int slotCacheSize = value;
				Legacy().PrefetchSlotCacheSize(slotCacheSize);
			}
		}

		public virtual int TimeoutClientSocket
		{
			get
			{
				return Legacy().TimeoutClientSocket();
			}
			set
			{
				int milliseconds = value;
				Legacy().TimeoutClientSocket(milliseconds);
			}
		}

		public virtual void AddConfigurationItem(IClientConfigurationItem configItem)
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

		public virtual void ApplyConfigurationItems(IExtClient client)
		{
			if (_configItems == null)
			{
				return;
			}
			for (IEnumerator configItemIter = _configItems.GetEnumerator(); configItemIter.MoveNext
				(); )
			{
				IClientConfigurationItem configItem = ((IClientConfigurationItem)configItemIter.Current
					);
				configItem.Apply(client);
			}
		}
	}
}
