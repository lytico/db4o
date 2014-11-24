/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
	public class MonitoredServerSocket4 : ServerSocket4Decorator
	{
		public MonitoredServerSocket4(IServerSocket4 serverSocket) : base(serverSocket)
		{
			_serverSocket = serverSocket;
		}

		public override ISocket4 Accept()
		{
			return new MonitoredServerSideClientSocket4(_serverSocket.Accept(), _counters);
		}

		public override void Close()
		{
			base.Close();
			_counters.Close();
		}

		private readonly NetworkingCounters _counters = new NetworkingCounters();
	}
}

#endif