/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MObjectSetFetch : MObjectSet, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			int queryResultID = ReadInt();
			int fetchSize = ReadInt();
			int fetchDepth = ReadInt();
			MsgD message = null;
			lock (ContainerLock())
			{
				IIntIterator4 idIterator = Stub(queryResultID).IdIterator();
				ByteArrayBuffer payload = ObjectExchangeStrategyFactory.ForConfig(new ObjectExchangeConfiguration
					(fetchDepth, fetchSize)).Marshall((LocalTransaction)Transaction(), idIterator, fetchSize
					);
				message = IdList.GetWriterForBuffer(Transaction(), payload);
			}
			return message;
		}
	}
}
