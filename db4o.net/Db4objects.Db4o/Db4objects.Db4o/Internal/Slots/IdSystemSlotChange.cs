/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class IdSystemSlotChange : SystemSlotChange
	{
		private Collection4 _freed;

		public IdSystemSlotChange(int id) : base(id)
		{
		}

		protected override void Free(IFreespaceManager freespaceManager, Slot slot)
		{
			if (slot.IsNull())
			{
				return;
			}
			if (_freed == null)
			{
				_freed = new Collection4();
			}
			_freed.Add(slot);
		}

		public override void AccumulateFreeSlot(TransactionalIdSystemImpl idSystem, FreespaceCommitter
			 freespaceCommitter, bool forFreespace)
		{
			if (ForFreespace() != forFreespace)
			{
				return;
			}
			base.AccumulateFreeSlot(idSystem, freespaceCommitter, forFreespace);
			if (_freed == null)
			{
				return;
			}
			IEnumerator iterator = _freed.GetEnumerator();
			while (iterator.MoveNext())
			{
				freespaceCommitter.DelayedFree((Slot)iterator.Current, FreeToSystemFreespaceSystem
					());
			}
		}

		protected override bool FreeToSystemFreespaceSystem()
		{
			return true;
		}
	}
}
