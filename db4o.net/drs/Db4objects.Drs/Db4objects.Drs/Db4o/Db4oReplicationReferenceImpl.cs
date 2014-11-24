/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Replication;
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Db4o
{
	/// <exclude></exclude>
	public class Db4oReplicationReferenceImpl : ObjectReference, IReplicationReference
		, IDb4oReplicationReference
	{
		private object _counterPart;

		private bool _markedForReplicating;

		private bool _markedForDeleting;

		private readonly long _version;

		internal Db4oReplicationReferenceImpl(IObjectInfo objectInfo, object obj)
		{
			if (obj == null)
			{
				throw new InvalidOperationException("obj may not be null");
			}
			_version = objectInfo.GetCommitTimestamp();
			ObjectReference @ref = (ObjectReference)objectInfo;
			Transaction trans = @ref.Transaction();
			Db4objects.Db4o.Internal.VirtualAttributes va = @ref.VirtualAttributes(trans);
			if (va != null)
			{
				SetVirtualAttributes((Db4objects.Db4o.Internal.VirtualAttributes)va.ShallowClone(
					));
			}
			else
			{
				SetVirtualAttributes(new Db4objects.Db4o.Internal.VirtualAttributes());
			}
			SetObject(obj);
			Ref_init();
		}

		public Db4oReplicationReferenceImpl(object obj, Db4oDatabase db, long longPart, long
			 version)
		{
			if (obj == null)
			{
				throw new InvalidOperationException("obj may not be null");
			}
			SetObject(obj);
			Ref_init();
			Db4objects.Db4o.Internal.VirtualAttributes va = new Db4objects.Db4o.Internal.VirtualAttributes
				();
			va.i_database = db;
			va.i_uuid = longPart;
			va.i_version = version;
			_version = version;
			SetVirtualAttributes(va);
		}

		public virtual Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl Add(Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl
			 newNode)
		{
			return (Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl)Hc_add(newNode);
		}

		public virtual Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl Find(object obj)
		{
			return (Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl)Hc_find(obj);
		}

		public virtual void Traverse(IVisitor4 visitor)
		{
			Hc_traverse(visitor);
		}

		public virtual IDrsUUID Uuid()
		{
			Db4oDatabase db = SignaturePart();
			if (db == null)
			{
				return null;
			}
			return new DrsUUIDImpl(new Db4oUUID(LongPart(), db.GetSignature()));
		}

		public virtual long Version()
		{
			return _version;
		}

		public virtual object Object()
		{
			return GetObject();
		}

		public virtual object Counterpart()
		{
			return _counterPart;
		}

		public virtual void SetCounterpart(object obj)
		{
			_counterPart = obj;
		}

		public virtual void MarkForReplicating(bool flag)
		{
			_markedForReplicating = flag;
		}

		public virtual bool IsMarkedForReplicating()
		{
			return _markedForReplicating;
		}

		public virtual void MarkForDeleting()
		{
			_markedForDeleting = true;
		}

		public virtual bool IsMarkedForDeleting()
		{
			return _markedForDeleting;
		}

		public virtual void MarkCounterpartAsNew()
		{
			throw new NotSupportedException("TODO");
		}

		public virtual bool IsCounterpartNew()
		{
			throw new NotSupportedException("TODO");
		}

		public virtual Db4oDatabase SignaturePart()
		{
			return VirtualAttributes().i_database;
		}

		public virtual long LongPart()
		{
			return VirtualAttributes().i_uuid;
		}

		public override Db4objects.Db4o.Internal.VirtualAttributes VirtualAttributes()
		{
			return VirtualAttributes(null);
		}

		public sealed override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o == null || o.GetType() != GetType())
			{
				return false;
			}
			Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl other = (Db4objects.Drs.Db4o.Db4oReplicationReferenceImpl
				)o;
			return Version() == other.Version() && Uuid().Equals(other.Uuid());
		}

		public sealed override int GetHashCode()
		{
			return 29 * Uuid().GetHashCode() + (int)(Version() ^ ((Version()) >> (32 & 0x1f))
				);
		}
	}
}
