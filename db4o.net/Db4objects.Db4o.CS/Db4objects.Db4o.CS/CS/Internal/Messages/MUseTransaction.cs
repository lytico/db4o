/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MUseTransaction : MsgD, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			IServerMessageDispatcher serverThread = ServerMessageDispatcher();
			serverThread.UseTransaction(this);
		}
	}
}
