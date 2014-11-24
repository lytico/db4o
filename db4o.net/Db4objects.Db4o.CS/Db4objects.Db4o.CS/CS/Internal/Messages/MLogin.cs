/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MLogin : MsgD, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				string userName = ReadString();
				string password = ReadString();
				ObjectServerImpl server = ServerMessageDispatcher().Server();
				User found = server.GetUser(userName);
				if (found != null)
				{
					if (found.password.Equals(password))
					{
						ServerMessageDispatcher().SetDispatcherName(userName);
						LogMsg(32, userName);
						int blockSize = Container().BlockSize();
						int encrypt = Container()._handlers.i_encrypt ? 1 : 0;
						ServerMessageDispatcher().Login();
						return Msg.LoginOk.GetWriterForInts(Transaction(), new int[] { blockSize, encrypt
							, ServerMessageDispatcher().DispatcherID() });
					}
				}
			}
			return Msg.Failed;
		}
	}
}
