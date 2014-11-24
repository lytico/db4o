/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MReadObject : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			StatefulBuffer bytes = null;
			// readWriterByID may fail in certain cases, for instance if
			// and object was deleted by another client
			int id = _payLoad.ReadInt();
			int lastCommitted = _payLoad.ReadInt();
			lock (ContainerLock())
			{
				bytes = Container().ReadStatefulBufferById(Transaction(), id, lastCommitted == 1);
			}
			if (bytes == null)
			{
				bytes = new StatefulBuffer(Transaction(), 0, 0);
			}
			return Msg.ObjectToClient.GetWriter(bytes);
		}
	}
}
