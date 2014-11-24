/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class SystemSlotChange : SlotChange
	{
		public SystemSlotChange(int id) : base(id)
		{
		}

		public override void AccumulateFreeSlot(TransactionalIdSystemImpl idSystem, FreespaceCommitter
			 freespaceCommitter, bool forFreespace)
		{
			base.AccumulateFreeSlot(idSystem, freespaceCommitter, forFreespace);
		}

		// FIXME: If we are doing a delete, we should also free our pointer here.
		protected override Slot ModifiedSlotInParentIdSystem(TransactionalIdSystemImpl idSystem
			)
		{
			return null;
		}

		public override bool RemoveId()
		{
			return _newSlot == Slot.Zero;
		}
	}
}
