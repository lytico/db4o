/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	public class DrsUUIDImpl : IDrsUUID
	{
		private readonly Signature _signature;

		private readonly long _longPart;

		public DrsUUIDImpl(Signature signature, long longPart)
		{
			_signature = signature;
			_longPart = longPart;
		}

		public DrsUUIDImpl(byte[] signature, long longPart) : this(new Signature(signature
			), longPart)
		{
		}

		public DrsUUIDImpl(Db4oUUID db4oUUID) : this(db4oUUID.GetSignaturePart(), db4oUUID
			.GetLongPart())
		{
		}

		public virtual long GetLongPart()
		{
			return _longPart;
		}

		public virtual byte[] GetSignaturePart()
		{
			return _signature.bytes;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Drs.Foundation.DrsUUIDImpl))
			{
				return false;
			}
			Db4objects.Drs.Foundation.DrsUUIDImpl other = (Db4objects.Drs.Foundation.DrsUUIDImpl
				)obj;
			return _longPart == other._longPart && _signature.Equals(other._signature);
		}

		public override int GetHashCode()
		{
			return ((int)_longPart) ^ _signature.GetHashCode();
		}

		public override string ToString()
		{
			return "longpart " + _longPart + " signature " + _signature.AsString();
		}
	}
}
