/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MPrefetchIDs : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			int prefetchIDCount = ReadInt();
			MsgD reply = Msg.IdList.GetWriterForLength(Transaction(), Const4.IntLength * prefetchIDCount
				);
			lock (ContainerLock())
			{
				ITransactionalIdSystem idSystem = Transaction().IdSystem();
				for (int i = 0; i < prefetchIDCount; i++)
				{
					reply.WriteInt(idSystem.PrefetchID());
				}
			}
			return reply;
		}
	}
}
