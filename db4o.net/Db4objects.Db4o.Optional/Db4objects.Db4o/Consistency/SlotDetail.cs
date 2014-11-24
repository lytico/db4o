/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Consistency
{
	public abstract class SlotDetail
	{
		public readonly Slot _slot;

		public SlotDetail(Slot slot)
		{
			this._slot = slot;
		}
	}
}
