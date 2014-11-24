/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Tests.Optional.Monitoring.CS;

namespace Db4objects.Db4o.Tests.Optional.Monitoring.CS
{
	public class CountingServerSocket4 : IServerSocket4
	{
		public CountingServerSocket4(IServerSocket4 serverSocket)
		{
			_serverSocket = serverSocket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 Accept()
		{
			CountingSocket4 socket = new CountingSocket4(_serverSocket.Accept());
			_clients.Add(socket);
			return socket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			_serverSocket.Close();
		}

		public virtual int GetLocalPort()
		{
			return _serverSocket.GetLocalPort();
		}

		public virtual void SetSoTimeout(int timeout)
		{
			_serverSocket.SetSoTimeout(timeout);
		}

		public virtual IList ConnectedClients()
		{
			return _clients;
		}

		private IServerSocket4 _serverSocket;

		private IList _clients = new ArrayList();
	}
}
#endif // !SILVERLIGHT
