/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MTaDelete : MsgD, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			int id = _payLoad.ReadInt();
			int cascade = _payLoad.ReadInt();
			Transaction trans = Transaction();
			lock (ContainerLock())
			{
				trans.Delete(null, id, cascade);
			}
		}
	}
}
