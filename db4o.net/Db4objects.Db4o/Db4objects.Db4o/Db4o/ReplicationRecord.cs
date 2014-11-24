/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Drs.Db4o
{
	/// <summary>
	/// tracks the version of the last replication between
	/// two Objectcontainers.
	/// </summary>
	/// <remarks>
	/// tracks the version of the last replication between
	/// two Objectcontainers.
	/// This class is persistent. It can't be moved to a
	/// different package and field names can not be changed.
	/// </remarks>
	/// <exclude></exclude>
	/// <persistent></persistent>
	public class ReplicationRecord : IInternal4
	{
		public Db4oDatabase _youngerPeer;

		public Db4oDatabase _olderPeer;

		public long _version;

		public long[] _concurrentTimestamps;

		public ReplicationRecord()
		{
		}

		public ReplicationRecord(Db4oDatabase younger, Db4oDatabase older)
		{
			_youngerPeer = younger;
			_olderPeer = older;
		}

		public virtual void SetVersion(long version)
		{
			_version = version;
		}

		public virtual void Store(ObjectContainerBase container)
		{
			Store(container.CheckTransaction());
		}

		public virtual void Store(Transaction trans)
		{
			ObjectContainerBase container = trans.Container();
			container.ShowInternalClasses(true);
			try
			{
				container.StoreAfterReplication(trans, this, container.UpdateDepthProvider().ForDepth
					(int.MaxValue), false);
			}
			finally
			{
				container.ShowInternalClasses(false);
			}
		}

		public static Db4objects.Drs.Db4o.ReplicationRecord BeginReplication(Transaction 
			transA, Transaction transB)
		{
			ObjectContainerBase peerA = transA.Container();
			ObjectContainerBase peerB = transB.Container();
			Db4oDatabase dbA = ((IInternalObjectContainer)peerA).Identity();
			Db4oDatabase dbB = ((IInternalObjectContainer)peerB).Identity();
			dbB.Bind(transA);
			dbA.Bind(transB);
			Db4oDatabase younger = null;
			Db4oDatabase older = null;
			if (dbA.IsOlderThan(dbB))
			{
				younger = dbB;
				older = dbA;
			}
			else
			{
				younger = dbA;
				older = dbB;
			}
			Db4objects.Drs.Db4o.ReplicationRecord rrA = QueryForReplicationRecord(peerA, transA
				, younger, older);
			Db4objects.Drs.Db4o.ReplicationRecord rrB = QueryForReplicationRecord(peerB, transB
				, younger, older);
			if (rrA == null)
			{
				if (rrB == null)
				{
					return new Db4objects.Drs.Db4o.ReplicationRecord(younger, older);
				}
				rrB.Store(peerA);
				return rrB;
			}
			if (rrB == null)
			{
				rrA.Store(peerB);
				return rrA;
			}
			if (rrA != rrB)
			{
				peerB.ShowInternalClasses(true);
				try
				{
					int id = peerB.GetID(transB, rrB);
					peerB.Bind(transB, rrA, id);
				}
				finally
				{
					peerB.ShowInternalClasses(false);
				}
			}
			return rrA;
		}

		public static Db4objects.Drs.Db4o.ReplicationRecord QueryForReplicationRecord(ObjectContainerBase
			 container, Transaction trans, Db4oDatabase younger, Db4oDatabase older)
		{
			container.ShowInternalClasses(true);
			try
			{
				IQuery q = container.Query(trans);
				q.Constrain(typeof(Db4objects.Drs.Db4o.ReplicationRecord));
				q.Descend("_youngerPeer").Constrain(younger).Identity();
				q.Descend("_olderPeer").Constrain(older).Identity();
				IObjectSet objectSet = q.Execute();
				if (objectSet.HasNext())
				{
					Db4objects.Drs.Db4o.ReplicationRecord replicationRecord = (Db4objects.Drs.Db4o.ReplicationRecord
						)objectSet.Next();
					container.Activate(replicationRecord, int.MaxValue);
					return replicationRecord;
				}
				return null;
			}
			finally
			{
				container.ShowInternalClasses(false);
			}
		}

		public virtual void ConcurrentTimestamps(IList concurrentTimestamps)
		{
			_concurrentTimestamps = Arrays4.ToLongArray(concurrentTimestamps);
		}

		public virtual long[] ConcurrentTimestamps()
		{
			return _concurrentTimestamps;
		}
	}
}
