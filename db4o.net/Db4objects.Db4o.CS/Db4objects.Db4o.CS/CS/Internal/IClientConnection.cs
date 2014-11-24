/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.CS.Internal
{
	public interface IClientConnection
	{
		string Name
		{
			get;
		}

		event System.EventHandler<MessageEventArgs> MessageReceived;
	}
}
