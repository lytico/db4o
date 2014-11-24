/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MGenerateTransactionTimestamp : MsgD, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			long forcedTimestamp = ReadLong();
			long timestamp = Transaction().GenerateTransactionTimestamp(forcedTimestamp);
			return Msg.GenerateTransactionTimestamp.GetWriterForLong(Transaction(), timestamp
				);
		}
	}
}
