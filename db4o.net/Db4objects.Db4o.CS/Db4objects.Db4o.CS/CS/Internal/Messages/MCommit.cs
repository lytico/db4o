/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MCommit : MsgD, IMessageWithResponse
	{
		protected CallbackObjectInfoCollections committedInfo = null;

		public virtual Msg ReplyFromServer()
		{
			IServerMessageDispatcher dispatcher = ServerMessageDispatcher();
			lock (ContainerLock())
			{
				ServerTransaction().Commit(dispatcher);
				committedInfo = dispatcher.CommittedInfo();
			}
			return Msg.Ok;
		}

		public override void PostProcessAtServer()
		{
			try
			{
				if (committedInfo != null)
				{
					AddCommittedInfoMsg(committedInfo, ServerTransaction());
				}
			}
			catch (Exception exc)
			{
				Sharpen.Runtime.PrintStackTrace(exc);
			}
		}

		private void AddCommittedInfoMsg(CallbackObjectInfoCollections committedInfo, LocalTransaction
			 serverTransaction)
		{
			lock (ContainerLock())
			{
				Msg.CommittedInfo.SetTransaction(serverTransaction);
				MCommittedInfo message = Msg.CommittedInfo.Encode(committedInfo, ServerMessageDispatcher
					().DispatcherID());
				message.SetMessageDispatcher(ServerMessageDispatcher());
				ServerMessageDispatcher().Server().AddCommittedInfoMsg(message);
			}
		}
	}
}
