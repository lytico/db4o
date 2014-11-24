/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MVersion : Msg, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			long ver = 0;
			lock (ContainerLock())
			{
				ver = Container().CurrentVersion();
			}
			return Msg.IdList.GetWriterForLong(Transaction(), ver);
		}
	}
}
