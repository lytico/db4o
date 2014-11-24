/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public class AddressKeySlotHandler : SlotHandler
	{
		public virtual int CompareTo(object obj)
		{
			return _current.CompareByAddress((Slot)obj);
		}

		public override IPreparedComparison PrepareComparison(IContext context, object slot
			)
		{
			Slot sourceSlot = (Slot)slot;
			return new _IPreparedComparison_21(sourceSlot);
		}

		private sealed class _IPreparedComparison_21 : IPreparedComparison
		{
			public _IPreparedComparison_21(Slot sourceSlot)
			{
				this.sourceSlot = sourceSlot;
			}

			public int CompareTo(object obj)
			{
				Slot targetSlot = (Slot)obj;
				// FIXME: The comparison method in #compareByAddress is the wrong way around.
				// Fix there and here after other references are fixed.
				return -sourceSlot.CompareByAddress(targetSlot);
			}

			private readonly Slot sourceSlot;
		}
	}
}
