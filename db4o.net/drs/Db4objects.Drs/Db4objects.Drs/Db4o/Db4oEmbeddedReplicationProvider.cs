/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Replication;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.TA;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Db4o
{
	public class Db4oEmbeddedReplicationProvider : IDb4oReplicationProvider
	{
		private IReadonlyReplicationProviderSignature _mySignature;

		protected readonly ExternalObjectContainer _container;

		private readonly IReflector _reflector;

		private ReplicationRecord _replicationRecord;

		internal Db4oReplicationReferenceImpl _referencesByObject;

		private Db4oSignatureMap _signatureMap;

		private readonly string _name;

		private readonly IProcedure4 _activationStrategy;

		protected long _commitTimestamp;

		public Db4oEmbeddedReplicationProvider(IObjectContainer objectContainer, string name
			)
		{
			// TODO: Add additional query methods (whereModified )
			IConfiguration cfg = objectContainer.Ext().Configure();
			cfg.Callbacks(false);
			_name = name;
			_container = (ExternalObjectContainer)objectContainer;
			_reflector = _container.Reflector();
			_signatureMap = new Db4oSignatureMap(_container);
			_activationStrategy = CreateActivationStrategy();
		}

		public Db4oEmbeddedReplicationProvider(IObjectContainer objectContainer) : this(objectContainer
			, objectContainer.ToString())
		{
		}

		private IProcedure4 CreateActivationStrategy()
		{
			if (IsTransparentActivationEnabled())
			{
				return new _IProcedure4_86(this);
			}
			return new _IProcedure4_94(this);
		}

		private sealed class _IProcedure4_86 : IProcedure4
		{
			public _IProcedure4_86(Db4oEmbeddedReplicationProvider _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object obj)
			{
				IObjectInfo objectInfo = this._enclosing._container.GetObjectInfo(obj);
				((IActivator)objectInfo).Activate(ActivationPurpose.Read);
			}

			private readonly Db4oEmbeddedReplicationProvider _enclosing;
		}

		private sealed class _IProcedure4_94 : IProcedure4
		{
			public _IProcedure4_94(Db4oEmbeddedReplicationProvider _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object obj)
			{
				if (obj == null)
				{
					return;
				}
				IReflectClass claxx = this._enclosing._reflector.ForObject(obj);
				int level = claxx.IsCollection() ? 3 : 1;
				this._enclosing._container.Activate(obj, level);
			}

			private readonly Db4oEmbeddedReplicationProvider _enclosing;
		}

		private bool IsTransparentActivationEnabled()
		{
			return TransparentActivationSupport.IsTransparentActivationEnabledOn(_container);
		}

		public virtual IReadonlyReplicationProviderSignature GetSignature()
		{
			if (_mySignature == null)
			{
				_mySignature = new Db4oReplicationProviderSignature(_container.Identity());
			}
			return _mySignature;
		}

		private object Lock()
		{
			return _container.Lock();
		}

		public virtual void StartReplicationTransaction(IReadonlyReplicationProviderSignature
			 peerSignature)
		{
			ClearAllReferences();
			lock (Lock())
			{
				Transaction trans = _container.Transaction;
				Db4oDatabase myIdentity = _container.Identity();
				_signatureMap.Put(myIdentity);
				Db4oDatabase otherIdentity = _signatureMap.Produce(peerSignature.GetSignature(), 
					peerSignature.GetCreated());
				Db4oDatabase younger = null;
				Db4oDatabase older = null;
				if (myIdentity.IsOlderThan(otherIdentity))
				{
					younger = otherIdentity;
					older = myIdentity;
				}
				else
				{
					younger = myIdentity;
					older = otherIdentity;
				}
				_replicationRecord = ReplicationRecord.QueryForReplicationRecord(_container, trans
					, younger, older);
				if (_replicationRecord == null)
				{
					_replicationRecord = new ReplicationRecord(younger, older);
					_replicationRecord.Store(_container);
				}
				else
				{
					_container.RaiseCommitTimestamp(_replicationRecord._version + 1);
				}
				_commitTimestamp = _container.GenerateTransactionTimestamp(0);
			}
		}

		public virtual void CommitReplicationTransaction()
		{
			StoreReplicationRecord();
			_container.Commit();
			_container.UseDefaultTransactionTimestamp();
		}

		protected virtual void StoreReplicationRecord()
		{
			_replicationRecord._version = _commitTimestamp;
			_replicationRecord.Store(_container);
		}

		protected virtual long ReplicationRecordId()
		{
			return _container.GetID(_replicationRecord);
		}

		public virtual void RollbackReplication()
		{
			_container.Rollback();
			_referencesByObject = null;
		}

		public virtual long GetLastReplicationVersion()
		{
			return _replicationRecord._version;
		}

		public virtual void StoreReplica(object obj)
		{
			Logger4Support.LogIdentity(obj, GetName());
			lock (Lock())
			{
				_container.StoreByNewReplication(this, obj);
			}
		}

		public virtual void Activate(object obj)
		{
			_activationStrategy.Apply(obj);
		}

		public virtual IDb4oReplicationReference ReferenceFor(object obj)
		{
			if (_referencesByObject == null)
			{
				return null;
			}
			return _referencesByObject.Find(obj);
		}

		public virtual IReplicationReference ProduceReference(object obj, object unused, 
			string unused2)
		{
			if (obj == null)
			{
				return null;
			}
			if (_referencesByObject != null)
			{
				Db4oReplicationReferenceImpl existingNode = _referencesByObject.Find(obj);
				if (existingNode != null)
				{
					return existingNode;
				}
			}
			// TODO: Why refresh here? Try without and run all tests!
			Refresh(obj);
			IObjectInfo objectInfo = _container.GetObjectInfo(obj);
			if (objectInfo == null)
			{
				return null;
			}
			Db4oUUID uuid = objectInfo.GetUUID();
			if (uuid == null)
			{
				throw new ArgumentNullException();
			}
			Db4oReplicationReferenceImpl newNode = new Db4oReplicationReferenceImpl(objectInfo
				, obj);
			AddReference(newNode);
			return newNode;
		}

		protected virtual void Refresh(object obj)
		{
		}

		//empty in File Provider
		private void AddReference(Db4oReplicationReferenceImpl newNode)
		{
			if (_referencesByObject == null)
			{
				_referencesByObject = newNode;
			}
			else
			{
				_referencesByObject = _referencesByObject.Add(newNode);
			}
		}

		public virtual IReplicationReference ReferenceNewObject(object obj, IReplicationReference
			 counterpartReference, IReplicationReference referencingObjCounterPartRef, string
			 fieldName)
		{
			IDrsUUID uuid = counterpartReference.Uuid();
			if (uuid == null)
			{
				return null;
			}
			byte[] signature = uuid.GetSignaturePart();
			long longPart = uuid.GetLongPart();
			long version = counterpartReference.Version();
			Db4oDatabase db = _signatureMap.Produce(signature, 0);
			Db4oReplicationReferenceImpl @ref = new Db4oReplicationReferenceImpl(obj, db, longPart
				, version);
			AddReference(@ref);
			return @ref;
		}

		public virtual IReplicationReference ProduceReferenceByUUID(IDrsUUID uuid, Type hint
			)
		{
			if (uuid == null)
			{
				return null;
			}
			object obj = _container.GetByUUID(new Db4oUUID(uuid.GetLongPart(), uuid.GetSignaturePart
				()));
			if (obj == null)
			{
				return null;
			}
			if (!_container.IsActive(obj))
			{
				_container.Activate(obj, 1);
			}
			return ProduceReference(obj);
		}

		public virtual void VisitCachedReferences(IVisitor4 visitor)
		{
			if (_referencesByObject != null)
			{
				_referencesByObject.Traverse(new _IVisitor4_287(visitor));
			}
		}

		private sealed class _IVisitor4_287 : IVisitor4
		{
			public _IVisitor4_287(IVisitor4 visitor)
			{
				this.visitor = visitor;
			}

			public void Visit(object obj)
			{
				Db4oReplicationReferenceImpl node = (Db4oReplicationReferenceImpl)obj;
				visitor.Visit(node);
			}

			private readonly IVisitor4 visitor;
		}

		public virtual void ClearAllReferences()
		{
			_referencesByObject = null;
		}

		public virtual IObjectSet ObjectsChangedSinceLastReplication()
		{
			IQuery q = _container.Query();
			WhereModified(q);
			return q.Execute();
		}

		public virtual IObjectSet ObjectsChangedSinceLastReplication(Type clazz)
		{
			IQuery q = _container.Query();
			q.Constrain(clazz);
			WhereModified(q);
			return q.Execute();
		}

		/// <summary>
		/// adds a constraint to the passed Query to query only for objects that were
		/// modified since the last replication process between this and the other
		/// ObjectContainer involved in the current replication process.
		/// </summary>
		/// <remarks>
		/// adds a constraint to the passed Query to query only for objects that were
		/// modified since the last replication process between this and the other
		/// ObjectContainer involved in the current replication process.
		/// </remarks>
		/// <param name="query">the Query to be constrained</param>
		public virtual void WhereModified(IQuery query)
		{
			IQuery qTimestamp = query.Descend(VirtualField.CommitTimestamp);
			IConstraint constraint = qTimestamp.Constrain(GetLastReplicationVersion()).Greater
				();
			long[] concurrentTimestamps = _replicationRecord._concurrentTimestamps;
			if (concurrentTimestamps != null)
			{
				for (int i = 0; i < concurrentTimestamps.Length; i++)
				{
					constraint = constraint.Or(qTimestamp.Constrain(concurrentTimestamps[i]));
				}
			}
		}

		public virtual IObjectSet GetStoredObjects(Type type)
		{
			IQuery query = _container.Query();
			query.Constrain(type);
			return query.Execute();
		}

		public virtual void StoreNew(object o)
		{
			Logger4Support.LogIdentity(o, GetName());
			_container.Store(o);
		}

		public virtual void Update(object o)
		{
			_container.Store(o);
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual void Destroy()
		{
		}

		// do nothing
		public virtual void Commit()
		{
			_container.Commit();
		}

		public virtual void DeleteAllInstances(Type clazz)
		{
			IQuery q = _container.Query();
			q.Constrain(clazz);
			IEnumerator objectSet = q.Execute().GetEnumerator();
			while (objectSet.MoveNext())
			{
				Delete(objectSet.Current);
			}
		}

		public virtual void Delete(object obj)
		{
			_container.Delete(obj);
		}

		public virtual bool WasModifiedSinceLastReplication(IReplicationReference reference
			)
		{
			long timestamp = reference.Version();
			if (timestamp > _replicationRecord._version)
			{
				return true;
			}
			long[] concurrentTimestamps = _replicationRecord.ConcurrentTimestamps();
			if (concurrentTimestamps == null)
			{
				return false;
			}
			for (int i = 0; i < concurrentTimestamps.Length; i++)
			{
				if (timestamp == concurrentTimestamps[i])
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool SupportsMultiDimensionalArrays()
		{
			return true;
		}

		public virtual bool SupportsHybridCollection()
		{
			return true;
		}

		public virtual bool SupportsRollback()
		{
			return false;
		}

		public override string ToString()
		{
			return GetName();
		}

		public virtual void ReplicateDeletion(IDrsUUID uuid)
		{
			object obj = _container.GetByUUID(new Db4oUUID(uuid.GetLongPart(), uuid.GetSignaturePart
				()));
			if (obj == null)
			{
				return;
			}
			_container.Delete(obj);
		}

		public virtual IExtObjectContainer GetObjectContainer()
		{
			return _container;
		}

		public virtual bool IsProviderSpecific(object original)
		{
			return false;
		}

		public virtual void ReplicationReflector(Db4objects.Drs.Inside.ReplicationReflector
			 replicationReflector)
		{
		}

		public virtual IObjectSet GetStoredObjects()
		{
			return _container.Query().Execute();
		}

		public virtual IReplicationReference ProduceReference(object obj)
		{
			return ProduceReference(obj, null, null);
		}

		public virtual object ReplaceIfSpecific(object value)
		{
			return value;
		}

		public virtual bool IsSecondClassObject(object obj)
		{
			return false;
		}

		public virtual long ObjectVersion(object @object)
		{
			return _container.GetObjectInfo(@object).GetCommitTimestamp();
		}

		public virtual long CreationTime(object @object)
		{
			return _container.GetObjectInfo(@object).GetUUID().GetLongPart();
		}

		public virtual void EnsureVersionsAreGenerated()
		{
			Commit();
		}

		public virtual Db4objects.Drs.Foundation.TimeStamps TimeStamps()
		{
			return new Db4objects.Drs.Foundation.TimeStamps(_replicationRecord._version, _commitTimestamp
				);
		}

		public virtual void WaitForPreviousCommits()
		{
		}

		// do nothing
		public virtual void SyncCommitTimestamp(long syncedTimeStamp)
		{
			if (syncedTimeStamp <= _commitTimestamp)
			{
				return;
			}
			_commitTimestamp = syncedTimeStamp;
			_container.RaiseCommitTimestamp(syncedTimeStamp + 1);
			_container.GenerateTransactionTimestamp(syncedTimeStamp);
		}
	}
}
