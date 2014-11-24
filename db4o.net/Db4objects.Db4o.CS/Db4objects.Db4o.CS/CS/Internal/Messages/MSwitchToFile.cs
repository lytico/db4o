/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MSwitchToFile : MsgD, IServerSideMessage
	{
		public virtual void ProcessAtServer()
		{
			IServerMessageDispatcher serverThread = ServerMessageDispatcher();
			serverThread.SwitchToFile(this);
		}
	}
}
