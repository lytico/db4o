/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MVersionForId : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				long version = SystemTransaction().VersionForId(ReadInt());
				return Msg.VersionForId.GetWriterForLong(Transaction(), version);
			}
		}
	}
}
