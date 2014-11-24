/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>Configuration interface for db4o networking clients.</summary>
	/// <remarks>Configuration interface for db4o networking clients.</remarks>
	/// <since>7.5</since>
	public interface IClientConfiguration : INetworkingConfigurationProvider, ICommonConfigurationProvider
	{
		/// <summary>
		/// Sets the number of IDs to be pre-allocated in the database for new
		/// objects created on the client.
		/// </summary>
		/// <remarks>
		/// Sets the number of IDs to be pre-allocated in the database for new
		/// objects created on the client.
		/// </remarks>
		/// <value>The number of IDs to be prefetched</value>
		int PrefetchIDCount
		{
			set;
		}

		/// <summary>Sets the number of objects to be prefetched for an ObjectSet.</summary>
		/// <remarks>Sets the number of objects to be prefetched for an ObjectSet.</remarks>
		/// <value>The number of objects to be prefetched</value>
		int PrefetchObjectCount
		{
			set;
		}

		/// <summary>returns the MessageSender for this Configuration context.</summary>
		/// <remarks>
		/// returns the MessageSender for this Configuration context.
		/// This setting should be used on the client side.
		/// </remarks>
		/// <returns>MessageSender</returns>
		IMessageSender MessageSender
		{
			get;
		}

		/// <summary>Sets the depth to which prefetched objects will be activated.</summary>
		/// <remarks>Sets the depth to which prefetched objects will be activated.</remarks>
		int PrefetchDepth
		{
			set;
		}

		/// <summary>Sets the slot cache size to the given value.</summary>
		/// <remarks>Sets the slot cache size to the given value.</remarks>
		/// <value></value>
		int PrefetchSlotCacheSize
		{
			set;
		}

		/// <summary>
		/// configures the time a client waits for a message response
		/// from the server.
		/// </summary>
		/// <remarks>
		/// configures the time a client waits for a message response
		/// from the server. <br />
		/// <br />
		/// Default value: 600000ms (10 minutes)<br />
		/// <br />
		/// It is recommended to use the same values for
		/// <see cref="TimeoutClientSocket(int)">TimeoutClientSocket(int)</see>
		/// and
		/// <see cref="IServerConfiguration.TimeoutServerSocket(int)">IServerConfiguration.TimeoutServerSocket(int)
		/// 	</see>
		/// .
		/// <br />
		/// </remarks>
		/// <value>time in milliseconds</value>
		int TimeoutClientSocket
		{
			set;
		}

		/// <summary>
		/// adds ConfigurationItems to be applied when
		/// a networking
		/// <see cref="Db4objects.Db4o.CS.Internal.ClientObjectContainer">Db4objects.Db4o.CS.Internal.ClientObjectContainer
		/// 	</see>
		/// is opened.
		/// </summary>
		/// <param name="configItem">
		/// the
		/// <see cref="IClientConfigurationItem">IClientConfigurationItem</see>
		/// </param>
		/// <since>7.12</since>
		void AddConfigurationItem(IClientConfigurationItem configItem);
	}
}
