/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MCommittedCallBackRegistry : Msg, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			IServerMessageDispatcher dispatcher = ServerMessageDispatcher();
			dispatcher.CaresAboutCommitted(true);
			return Msg.Ok;
		}
	}
}
