/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class Pointer4
	{
		public readonly int _id;

		public readonly Slot _slot;

		public Pointer4(int id, Slot slot)
		{
			_id = id;
			_slot = slot;
		}

		public virtual int Address()
		{
			return _slot.Address();
		}

		public virtual int Id()
		{
			return _id;
		}

		public virtual int Length()
		{
			return _slot.Length();
		}
	}
}
