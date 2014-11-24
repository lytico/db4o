/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MObjectSetSize : MObjectSet, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				AbstractQueryResult queryResult = QueryResult(ReadInt());
				return Msg.ObjectsetSize.GetWriterForInt(Transaction(), queryResult.Size());
			}
		}
	}
}
