/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.Config
{
	/// <summary>Client/Server configuration interface.</summary>
	/// <remarks>Client/Server configuration interface.</remarks>
	public interface IClientServerConfiguration
	{
		/// <summary>
		/// Sets the number of IDs to be pre-allocated in the database for new
		/// objects created on the client.
		/// </summary>
		/// <remarks>
		/// Sets the number of IDs to be pre-allocated in the database for new
		/// objects created on the client.
		/// This setting should be used on the client side. In embedded mode this setting
		/// has no effect.
		/// </remarks>
		/// <param name="prefetchIDCount">The number of IDs to be prefetched</param>
		void PrefetchIDCount(int prefetchIDCount);

		/// <summary>Sets the number of objects to be prefetched for an ObjectSet.</summary>
		/// <remarks>
		/// Sets the number of objects to be prefetched for an ObjectSet.
		/// This setting should be used on the server side.
		/// </remarks>
		/// <param name="prefetchObjectCount">The number of objects to be prefetched</param>
		void PrefetchObjectCount(int prefetchObjectCount);

		/// <summary>Sets the depth to which prefetched objects are activated.</summary>
		/// <remarks>
		/// Sets the depth to which prefetched objects are activated.
		/// This setting should be used on the client side.
		/// </remarks>
		/// <param name="prefetchDepth"></param>
		void PrefetchDepth(int prefetchDepth);

		/// <summary>Sets the slot cache size to the given value.</summary>
		/// <remarks>Sets the slot cache size to the given value.</remarks>
		/// <param name="slotCacheSize"></param>
		void PrefetchSlotCacheSize(int slotCacheSize);

		/// <summary>sets the MessageRecipient to receive Client Server messages.</summary>
		/// <remarks>
		/// sets the MessageRecipient to receive Client Server messages. <br />
		/// <br />
		/// This setting should be used on the server side.<br /><br />
		/// </remarks>
		/// <param name="messageRecipient">the MessageRecipient to be used</param>
		void SetMessageRecipient(IMessageRecipient messageRecipient);

		/// <summary>returns the MessageSender for this Configuration context.</summary>
		/// <remarks>
		/// returns the MessageSender for this Configuration context.
		/// This setting should be used on the client side.
		/// </remarks>
		/// <returns>MessageSender</returns>
		IMessageSender GetMessageSender();

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
		/// <see cref="TimeoutServerSocket(int)">TimeoutServerSocket(int)</see>
		/// .
		/// <br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <param name="milliseconds">time in milliseconds</param>
		void TimeoutClientSocket(int milliseconds);

		/// <summary>configures the timeout of the serverside socket.</summary>
		/// <remarks>
		/// configures the timeout of the serverside socket. <br />
		/// <br />
		/// The serverside handler waits for messages to arrive from the client.
		/// If no more messages arrive for the duration configured in this
		/// setting, the client will be disconnected.
		/// <br />
		/// Clients send PING messages to the server at an interval of
		/// Math.min(timeoutClientSocket(), timeoutServerSocket()) / 2
		/// and the server will respond to keep connections alive.
		/// <br />
		/// Decrease this setting if you want clients to disconnect faster.
		/// <br />
		/// Increase this setting if you have a large number of clients and long
		/// running queries and you are getting disconnected clients that you
		/// would like to wait even longer for a response from the server.
		/// <br />
		/// Default value: 600000ms (10 minutes)<br />
		/// <br />
		/// It is recommended to use the same values for
		/// <see cref="TimeoutClientSocket(int)">TimeoutClientSocket(int)</see>
		/// and
		/// <see cref="TimeoutServerSocket(int)">TimeoutServerSocket(int)</see>
		/// .
		/// <br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <param name="milliseconds">time in milliseconds</param>
		void TimeoutServerSocket(int milliseconds);

		/// <summary>
		/// configures the client messaging system to be single threaded
		/// or multithreaded.
		/// </summary>
		/// <remarks>
		/// configures the client messaging system to be single threaded
		/// or multithreaded.
		/// <br /><br />Recommended settings:<br />
		/// - true for low resource systems.<br />
		/// - false for best asynchronous performance and fast
		/// GUI response.
		/// <br /><br />Default value:<br />
		/// - .NET Compactframework: true<br />
		/// - all other platforms: false<br /><br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <param name="flag">the desired setting</param>
		void SingleThreadedClient(bool flag);

		/// <summary>Configures to batch messages between client and server.</summary>
		/// <remarks>
		/// Configures to batch messages between client and server. By default, batch
		/// mode is enabled.<br /><br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <param name="flag">false, to turn message batching off.</param>
		void BatchMessages(bool flag);

		/// <summary>Configures the maximum memory buffer size for batched message.</summary>
		/// <remarks>
		/// Configures the maximum memory buffer size for batched message. If the
		/// size of batched messages is greater than maxSize, batched
		/// messages will be sent to server.<br /><br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <param name="maxSize"></param>
		void MaxBatchQueueSize(int maxSize);
	}
}
