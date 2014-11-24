/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	public class NetworkSocket : NetworkSocketBase
	{
		/// <exception cref="System.IO.IOException"></exception>
		public NetworkSocket(string hostName, int port) : base(new Sharpen.Net.Socket(hostName
			, port), hostName)
		{
		}

		/// <exception cref="System.IO.IOException"></exception>
		public NetworkSocket(Sharpen.Net.Socket socket) : base(socket)
		{
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected override ISocket4 CreateParallelSocket(string hostName, int port)
		{
			return new Db4objects.Db4o.CS.Foundation.NetworkSocket(hostName, port);
		}
	}
}
