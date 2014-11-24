/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.CS.Monitoring
{
	public class Db4oClientServerPerformanceCounters : Db4oPerformanceCounters
	{
		public static PerformanceCounter CounterForNetworkingClientConnections(IObjectServer server)
		{
			PerformanceCounter clientConnections = NewDb4oCounter(PerformanceCounterSpec.NetClientConnections.Id, false);
			
			IObjectServerEvents serverEvents = (IObjectServerEvents) server;
			serverEvents.ClientConnected += delegate { clientConnections.Increment(); };
			serverEvents.ClientDisconnected += delegate { clientConnections.Decrement(); };

			return clientConnections;
		}

		/*
         * TODO: Remove 
         */

		private static PerformanceCounter NewDb4oCounter(string counterName, bool readOnly)
		{
			string instanceName = My<IObjectContainer>.Instance.ToString();
			return NewDb4oCounter(counterName, instanceName, readOnly);
		}
	}
}

#endif