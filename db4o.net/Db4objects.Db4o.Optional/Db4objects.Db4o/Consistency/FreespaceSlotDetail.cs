/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Consistency
{
	public class FreespaceSlotDetail : SlotDetail
	{
		public FreespaceSlotDetail(Slot slot) : base(slot)
		{
		}

		public override string ToString()
		{
			return "FRE: " + _slot;
		}
	}
}
