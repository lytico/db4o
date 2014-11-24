/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.CS.Internal.Config
{
	public class NetworkingConfigurationImpl : INetworkingConfiguration
	{
		protected readonly Config4Impl _config;

		internal NetworkingConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual Config4Impl Config()
		{
			return _config;
		}

		public virtual bool BatchMessages
		{
			set
			{
				bool flag = value;
				_config.BatchMessages(flag);
			}
		}

		public virtual int MaxBatchQueueSize
		{
			set
			{
				int maxSize = value;
				_config.MaxBatchQueueSize(maxSize);
			}
		}

		public virtual bool SingleThreadedClient
		{
			set
			{
				bool flag = value;
				_config.SingleThreadedClient(flag);
			}
		}

		public virtual IMessageRecipient MessageRecipient
		{
			set
			{
				IMessageRecipient messageRecipient = value;
				_config.SetMessageRecipient(messageRecipient);
			}
		}

		public virtual IClientServerFactory ClientServerFactory
		{
			get
			{
				IClientServerFactory configuredFactory = ((IClientServerFactory)My(typeof(IClientServerFactory
					)));
				if (null == configuredFactory)
				{
					return new StandardClientServerFactory();
				}
				return configuredFactory;
			}
			set
			{
				IClientServerFactory factory = value;
				_config.EnvironmentContributions().Add(factory);
			}
		}

		public virtual ISocket4Factory SocketFactory
		{
			get
			{
				ISocket4Factory configuredFactory = ((ISocket4Factory)My(typeof(ISocket4Factory))
					);
				if (null == configuredFactory)
				{
					return new StandardSocket4Factory();
				}
				return configuredFactory;
			}
			set
			{
				ISocket4Factory factory = value;
				_config.EnvironmentContributions().Add(factory);
			}
		}

		private object My(Type type)
		{
			IList environmentContributions = _config.EnvironmentContributions();
			for (int i = environmentContributions.Count - 1; i >= 0; i--)
			{
				object o = environmentContributions[i];
				if (type.IsInstanceOfType(o))
				{
					return (object)o;
				}
			}
			return null;
		}
	}
}
