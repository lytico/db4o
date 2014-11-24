/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MObjectSetIndexOf : MObjectSet, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			AbstractQueryResult queryResult = QueryResult(ReadInt());
			lock (ContainerLock())
			{
				int id = queryResult.IndexOf(ReadInt());
				return Msg.ObjectsetIndexof.GetWriterForInt(Transaction(), id);
			}
		}
	}
}
