/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MObjectSetGetId : MObjectSet, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			AbstractQueryResult queryResult = QueryResult(ReadInt());
			int id = 0;
			lock (ContainerLock())
			{
				id = queryResult.GetId(ReadInt());
			}
			return Msg.ObjectsetGetId.GetWriterForInt(Transaction(), id);
		}
	}
}
