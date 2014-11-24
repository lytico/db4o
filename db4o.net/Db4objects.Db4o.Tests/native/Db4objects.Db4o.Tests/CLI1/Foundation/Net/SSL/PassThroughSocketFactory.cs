/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL
{
	internal class PassThroughSocketFactory : ISocket4Factory
	{
		public PassThroughSocketFactory(ISocket4Factory delegating)
		{
			_delegating = delegating;	
		}

		public PassThroughSocketFactory()
		{
			_delegating = new StandardSocket4Factory();
		}

		public ISocket4 CreateSocket(string hostName, int port)
		{
			ISocket4 socket = _delegating.CreateSocket(hostName, port);
			_lastClientSocket = new PassThroughSocket(socket);
			
			return _lastClientSocket;
		}

		public IServerSocket4 CreateServerSocket(int port)
		{
			return _delegating.CreateServerSocket(port);
		}

		public PassThroughSocket LastClient
		{
			get { return _lastClientSocket; }
		}

		private readonly ISocket4Factory _delegating;
		private PassThroughSocket _lastClientSocket;
	}
}

#endif