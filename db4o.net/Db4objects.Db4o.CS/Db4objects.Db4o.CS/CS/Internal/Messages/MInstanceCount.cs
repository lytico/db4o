/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MInstanceCount : MsgD, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				ClassMetadata clazz = LocalContainer().ClassMetadataForID(ReadInt());
				return Msg.InstanceCount.GetWriterForInt(Transaction(), clazz.IndexEntryCount(Transaction
					()));
			}
		}
	}
}
