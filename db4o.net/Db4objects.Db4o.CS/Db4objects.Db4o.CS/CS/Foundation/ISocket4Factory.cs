/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	public interface ISocket4Factory
	{
		/// <exception cref="System.IO.IOException"></exception>
		ISocket4 CreateSocket(string hostName, int port);

		/// <exception cref="System.IO.IOException"></exception>
		IServerSocket4 CreateServerSocket(int port);
	}
}
