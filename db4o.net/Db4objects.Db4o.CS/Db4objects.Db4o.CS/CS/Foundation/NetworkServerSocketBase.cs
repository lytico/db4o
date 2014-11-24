/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Net.Sockets;
using Db4objects.Db4o.CS.Foundation;
using Sharpen.Net;

namespace Db4objects.Db4o.CS.Foundation
{
	public abstract class NetworkServerSocketBase : IServerSocket4
	{
		protected abstract ServerSocket Socket();

		public virtual void SetSoTimeout(int timeout)
		{
			try
			{
				Socket().SetSoTimeout(timeout);
			}
			catch (SocketException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		public virtual int GetLocalPort()
		{
			return Socket().GetLocalPort();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 Accept()
		{
			Sharpen.Net.Socket sock = Socket().Accept();
			// TODO: check connection permissions here
			return new NetworkSocket(sock);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			Socket().Close();
		}
	}
}
