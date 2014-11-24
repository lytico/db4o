/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class IdSlotMapping
	{
		public int _id;

		public int _address;

		public int _length;

		public IdSlotMapping(int id, int address, int length)
		{
			// persistent and indexed in DatabaseIdMapping, don't change the name
			_id = id;
			_address = address;
			_length = length;
		}

		public IdSlotMapping(int id, Db4objects.Db4o.Internal.Slots.Slot slot) : this(id, 
			slot.Address(), slot.Length())
		{
		}

		public virtual Db4objects.Db4o.Internal.Slots.Slot Slot()
		{
			return new Db4objects.Db4o.Internal.Slots.Slot(_address, _length);
		}

		public virtual void Write(ByteArrayBuffer buffer)
		{
			buffer.WriteInt(_id);
			buffer.WriteInt(_address);
			buffer.WriteInt(_length);
		}

		public static Db4objects.Db4o.Internal.Ids.IdSlotMapping Read(ByteArrayBuffer buffer
			)
		{
			return new Db4objects.Db4o.Internal.Ids.IdSlotMapping(buffer.ReadInt(), buffer.ReadInt
				(), buffer.ReadInt());
		}

		public override string ToString()
		{
			return string.Empty + _id + ":" + _address + "," + _length;
		}
	}
}
