/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	public class ReplicationReferenceImpl : IReplicationReference
	{
		private bool _objectIsNew;

		private readonly object _obj;

		private readonly IDrsUUID _uuid;

		private readonly long _version;

		private object _counterPart;

		private bool _markedForReplicating;

		private bool _markedForDeleting;

		public ReplicationReferenceImpl(object obj, IDrsUUID uuid, long version)
		{
			this._obj = obj;
			this._uuid = uuid;
			this._version = version;
		}

		public object Counterpart()
		{
			return _counterPart;
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
			Db4objects.Drs.Inside.ReplicationReferenceImpl other = (Db4objects.Drs.Inside.ReplicationReferenceImpl
				)o;
			return _version == other._version && _uuid.Equals(other._uuid);
		}

		public sealed override int GetHashCode()
		{
			return 29 * _uuid.GetHashCode() + (int)(_version ^ ((_version) >> (32 & 0x1f)));
		}

		public virtual bool IsCounterpartNew()
		{
			return _objectIsNew;
		}

		public bool IsMarkedForDeleting()
		{
			return _markedForDeleting;
		}

		public bool IsMarkedForReplicating()
		{
			return _markedForReplicating;
		}

		public virtual void MarkCounterpartAsNew()
		{
			_objectIsNew = true;
		}

		public void MarkForDeleting()
		{
			_markedForDeleting = true;
		}

		public void MarkForReplicating(bool flag)
		{
			_markedForReplicating = flag;
		}

		public object Object()
		{
			return _obj;
		}

		public void SetCounterpart(object obj)
		{
			_counterPart = obj;
		}

		public override string ToString()
		{
			return "ReplicationReferenceImpl{" + "_objectIsNew=" + _objectIsNew + ", _obj=" +
				 _obj + ", _uuid=" + _uuid + ", _version=" + _version + ", _counterPart=" + _counterPart
				 + ", _markedForReplicating=" + _markedForReplicating + ", _markedForDeleting=" 
				+ _markedForDeleting + '}';
		}

		public IDrsUUID Uuid()
		{
			return _uuid;
		}

		public long Version()
		{
			return _version;
		}
	}
}
