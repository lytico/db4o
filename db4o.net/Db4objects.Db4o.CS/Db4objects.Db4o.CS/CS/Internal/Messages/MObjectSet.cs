/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public abstract class MObjectSet : MsgD
	{
		protected virtual AbstractQueryResult QueryResult(int queryResultID)
		{
			return Stub(queryResultID).QueryResult();
		}

		protected virtual LazyClientObjectSetStub Stub(int queryResultID)
		{
			IServerMessageDispatcher serverThread = ServerMessageDispatcher();
			return serverThread.QueryResultForID(queryResultID);
		}
	}
}
