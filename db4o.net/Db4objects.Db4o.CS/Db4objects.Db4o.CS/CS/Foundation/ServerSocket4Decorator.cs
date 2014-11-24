/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	/// <since>7.12</since>
	public class ServerSocket4Decorator : IServerSocket4
	{
		public ServerSocket4Decorator(IServerSocket4 serverSocket)
		{
			_serverSocket = serverSocket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 Accept()
		{
			return _serverSocket.Accept();
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

		protected IServerSocket4 _serverSocket;
	}
}
