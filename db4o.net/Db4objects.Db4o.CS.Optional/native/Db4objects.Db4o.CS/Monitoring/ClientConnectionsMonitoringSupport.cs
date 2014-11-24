/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Monitoring
{
	public class ClientConnectionsMonitoringSupport : IServerConfigurationItem
	{
		public void Prepare(IServerConfiguration configuration)
		{
		}

		public void Apply(IObjectServer server)
		{
			PerformanceCounter clientConnections = null;

			ObjectContainerBase container = (ObjectContainerBase) server.Ext().ObjectContainer();
			container.WithEnvironment(delegate
			{
				clientConnections = Db4oClientServerPerformanceCounters.CounterForNetworkingClientConnections(server);
			});

			ServerEventsFor(server).Closed += delegate { clientConnections.RemoveInstance();};
		}

		private static IObjectServerEvents ServerEventsFor(IObjectServer server)
		{
			return ((IObjectServerEvents) server);
		}
	}
}

#endif