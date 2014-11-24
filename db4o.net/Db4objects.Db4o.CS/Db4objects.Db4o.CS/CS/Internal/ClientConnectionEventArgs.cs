/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public class ClientConnectionEventArgs : EventArgs
	{
		private readonly IClientConnection _connection;

		public ClientConnectionEventArgs(IClientConnection connection)
		{
			_connection = connection;
		}

		public virtual IClientConnection Connection
		{
			get
			{
				return _connection;
			}
		}
	}
}
