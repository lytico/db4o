/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;
using Sharpen.Net;

namespace Db4objects.Db4o.CS.Foundation
{
	public class NetworkServerSocket : NetworkServerSocketBase
	{
		private ServerSocket _socket;

		/// <exception cref="System.IO.IOException"></exception>
		public NetworkServerSocket(int port)
		{
			_socket = new ServerSocket(port);
		}

		protected override ServerSocket Socket()
		{
			return _socket;
		}
	}
}
