/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Drs.Db4o;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MCommitReplication : MCommit, IMessageWithResponse
	{
		public override Msg ReplyFromServer()
		{
			IServerMessageDispatcher dispatcher = ServerMessageDispatcher();
			lock (ContainerLock())
			{
				LocalTransaction trans = ServerTransaction();
				long replicationRecordId = ReadLong();
				long timestamp = ReadLong();
				IList concurrentTimestamps = trans.ConcurrentReplicationTimestamps();
				ServerMessageDispatcher().Server().BroadcastReplicationCommit(timestamp, concurrentTimestamps
					);
				ReplicationRecord replicationRecord = (ReplicationRecord)Container().GetByID(trans
					, replicationRecordId);
				Container().Activate(trans, replicationRecord, new FixedActivationDepth(int.MaxValue
					));
				replicationRecord.SetVersion(timestamp);
				replicationRecord.ConcurrentTimestamps(concurrentTimestamps);
				replicationRecord.Store(trans);
				Container().StoreAfterReplication(trans, replicationRecord, Container().UpdateDepthProvider
					().ForDepth(int.MaxValue), false);
				trans.Commit(dispatcher);
				committedInfo = dispatcher.CommittedInfo();
				Transaction().UseDefaultTransactionTimestamp();
			}
			return Msg.Ok;
		}
	}
}
