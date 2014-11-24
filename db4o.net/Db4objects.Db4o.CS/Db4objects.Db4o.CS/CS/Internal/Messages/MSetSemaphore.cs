/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MSetSemaphore : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			int timeout = ReadInt();
			string name = ReadString();
			bool res = LocalContainer().SetSemaphore(Transaction(), name, timeout);
			return (res ? (Msg)Msg.Success : Msg.Failed);
		}
	}
}
