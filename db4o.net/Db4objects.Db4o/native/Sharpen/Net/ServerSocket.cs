/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Net;
using System.Net.Sockets;
using NativeSocket=System.Net.Sockets.Socket;

namespace Sharpen.Net
{
	public class ServerSocket : SocketWrapper
	{
		public ServerSocket(int port)
		{
#if !SILVERLIGHT
			try
            {
				NativeSocket socket = new NativeSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));

                int maxPendingConnections = 42;
                socket.Listen(maxPendingConnections);
                Initialize(socket);
            }
            catch (SocketException e)
            {
                throw new System.IO.IOException(e.Message);
            }
#endif
		}

#if !SILVERLIGHT
		public Socket Accept()
		{
			return new Socket(_delegate.Accept());
		}

		public int GetLocalPort()
		{
			return ((IPEndPoint)_delegate.LocalEndPoint).Port;
		}
#endif
	}
}
