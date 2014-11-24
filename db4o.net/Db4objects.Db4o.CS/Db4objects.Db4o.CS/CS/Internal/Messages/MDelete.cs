/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MDelete : MsgD, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			ByteArrayBuffer bytes = this.GetByteLoad();
			lock (ContainerLock())
			{
				object obj = Container().TryGetByID(Transaction(), bytes.ReadInt());
				bool userCall = bytes.ReadInt() == 1;
				if (obj != null)
				{
					try
					{
						Container().Delete1(Transaction(), obj, userCall);
					}
					catch (Exception e)
					{
					}
				}
			}
		}
	}
}
