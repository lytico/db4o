/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.CS.Internal
{
	public interface IObjectServerEvents
	{
		event System.EventHandler<ClientConnectionEventArgs> ClientConnected;

		/// <returns>an event that provides the name of the client being disconnected.</returns>
		/// <since>7.12</since>
		event System.EventHandler<Db4objects.Db4o.Events.StringEventArgs> ClientDisconnected;

		event System.EventHandler<ServerClosedEventArgs> Closed;
	}
}
