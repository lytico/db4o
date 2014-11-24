/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MTaIsDeleted : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				bool isDeleted = Container().IsDeleted(Transaction(), ReadInt());
				int ret = isDeleted ? 1 : 0;
				return Msg.TaIsDeleted.GetWriterForInt(Transaction(), ret);
			}
		}
	}
}
