/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	public class StandardSocket4Factory : ISocket4Factory
	{
		/// <exception cref="System.IO.IOException"></exception>
		public virtual IServerSocket4 CreateServerSocket(int port)
		{
			return new NetworkServerSocket(port);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 CreateSocket(string hostName, int port)
		{
			return new NetworkSocket(hostName, port);
		}
	}
}
