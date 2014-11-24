/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Consistency
{
	public class IdObjectSlotDetail : SlotDetail
	{
		private readonly int _id;

		public IdObjectSlotDetail(int id, Slot slot) : base(slot)
		{
			_id = id;
		}

		public virtual int Id()
		{
			return _id;
		}

		public override string ToString()
		{
			return "OBJ: " + _slot + "(" + _id + ")";
		}
	}
}
