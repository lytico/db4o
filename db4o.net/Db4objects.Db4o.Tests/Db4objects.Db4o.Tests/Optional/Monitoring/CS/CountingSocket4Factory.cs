/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Tests.Optional.Monitoring.CS;

namespace Db4objects.Db4o.Tests.Optional.Monitoring.CS
{
	public class CountingSocket4Factory : ISocket4Factory
	{
		public CountingSocket4Factory(ISocket4Factory socketFactory)
		{
			_socketFactory = socketFactory;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual IServerSocket4 CreateServerSocket(int port)
		{
			_serverSocket = new CountingServerSocket4(_socketFactory.CreateServerSocket(port)
				);
			return _serverSocket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 CreateSocket(string hostName, int port)
		{
			CountingSocket4 socket = new CountingSocket4(_socketFactory.CreateSocket(hostName
				, port));
			_sockets.Add(socket);
			return socket;
		}

		public virtual IList CountingSockets()
		{
			return _sockets;
		}

		public virtual IList ConnectedClients()
		{
			return _serverSocket.ConnectedClients();
		}

		public virtual void ResetCounters()
		{
			for (IEnumerator socketIter = ConnectedClients().GetEnumerator(); socketIter.MoveNext
				(); )
			{
				CountingSocket4 socket = ((CountingSocket4)socketIter.Current);
				socket.ResetCount();
			}
		}

		private CountingServerSocket4 _serverSocket;

		private ISocket4Factory _socketFactory;

		private IList _sockets = new ArrayList();
	}
}
#endif // !SILVERLIGHT
