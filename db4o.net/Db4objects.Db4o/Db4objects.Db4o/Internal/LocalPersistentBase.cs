/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class LocalPersistentBase : PersistentBase
	{
		private readonly ITransactionalIdSystem _idSystem;

		public LocalPersistentBase(ITransactionalIdSystem idSystem)
		{
			_idSystem = idSystem;
		}

		public LocalPersistentBase() : this(null)
		{
		}

		public override ITransactionalIdSystem IdSystem(Transaction trans)
		{
			if (_idSystem != null)
			{
				return _idSystem;
			}
			return base.IdSystem(trans);
		}

		protected override ByteArrayBuffer ReadBufferById(Transaction trans)
		{
			Slot slot = IdSystem(trans).CurrentSlot(GetID());
			if (DTrace.enabled)
			{
				DTrace.SlotRead.LogLength(GetID(), slot);
			}
			return ((LocalObjectContainer)trans.Container()).ReadBufferBySlot(slot);
		}
	}
}
