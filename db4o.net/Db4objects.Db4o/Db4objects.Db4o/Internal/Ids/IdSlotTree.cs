/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class IdSlotTree : TreeInt
	{
		private readonly Db4objects.Db4o.Internal.Slots.Slot _slot;

		public IdSlotTree(int id, Db4objects.Db4o.Internal.Slots.Slot slot) : base(id)
		{
			_slot = slot;
		}

		public virtual Db4objects.Db4o.Internal.Slots.Slot Slot()
		{
			return _slot;
		}

		public override Tree OnAttemptToAddDuplicate(Tree oldNode)
		{
			_preceding = ((Tree)oldNode._preceding);
			_subsequent = ((Tree)oldNode._subsequent);
			_size = oldNode._size;
			return this;
		}

		public override int OwnLength()
		{
			return Const4.IntLength * 3;
		}

		// _key, _slot._address, _slot._length 
		public override object Read(ByteArrayBuffer buffer)
		{
			int id = buffer.ReadInt();
			Db4objects.Db4o.Internal.Slots.Slot slot = new Db4objects.Db4o.Internal.Slots.Slot
				(buffer.ReadInt(), buffer.ReadInt());
			return new Db4objects.Db4o.Internal.Ids.IdSlotTree(id, slot);
		}

		public override void Write(ByteArrayBuffer buffer)
		{
			buffer.WriteInt(_key);
			buffer.WriteInt(_slot.Address());
			buffer.WriteInt(_slot.Length());
		}
	}
}
