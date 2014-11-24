/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class Slot
	{
		private readonly int _address;

		private readonly int _length;

		public static readonly Db4objects.Db4o.Internal.Slots.Slot Zero = new Db4objects.Db4o.Internal.Slots.Slot
			(0, 0);

		public const int New = -1;

		public const int Update = -2;

		public Slot(int address, int length)
		{
			_address = address;
			_length = length;
		}

		public virtual int Address()
		{
			return _address;
		}

		public virtual int Length()
		{
			return _length;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (!(obj is Db4objects.Db4o.Internal.Slots.Slot))
			{
				return false;
			}
			Db4objects.Db4o.Internal.Slots.Slot other = (Db4objects.Db4o.Internal.Slots.Slot)
				obj;
			return (_address == other._address) && (Length() == other.Length());
		}

		public override int GetHashCode()
		{
			return _address ^ Length();
		}

		public virtual Db4objects.Db4o.Internal.Slots.Slot SubSlot(int offset)
		{
			return new Db4objects.Db4o.Internal.Slots.Slot(_address + offset, Length() - offset
				);
		}

		public override string ToString()
		{
			return "[A:" + _address + ",L:" + Length() + "]";
		}

		public virtual Db4objects.Db4o.Internal.Slots.Slot Truncate(int requiredLength)
		{
			return new Db4objects.Db4o.Internal.Slots.Slot(_address, requiredLength);
		}

		public static int MarshalledLength = Const4.IntLength * 2;

		public virtual int CompareByAddress(Db4objects.Db4o.Internal.Slots.Slot slot)
		{
			// FIXME: This is the wrong way around !!!
			// Fix here and in all referers.
			int res = slot._address - _address;
			if (res != 0)
			{
				return res;
			}
			return slot.Length() - Length();
		}

		public virtual int CompareByLength(Db4objects.Db4o.Internal.Slots.Slot slot)
		{
			// FIXME: This is the wrong way around !!!
			// Fix here and in all referers.
			int res = slot.Length() - Length();
			if (res != 0)
			{
				return res;
			}
			return slot._address - _address;
		}

		public virtual bool IsDirectlyPreceding(Db4objects.Db4o.Internal.Slots.Slot other
			)
		{
			return _address + Length() == other._address;
		}

		public virtual Db4objects.Db4o.Internal.Slots.Slot Append(Db4objects.Db4o.Internal.Slots.Slot
			 slot)
		{
			return new Db4objects.Db4o.Internal.Slots.Slot(Address(), _length + slot.Length()
				);
		}

		public virtual bool IsNull()
		{
			return Address() == 0 || Length() == 0;
		}

		public virtual bool IsNew()
		{
			return _address == New;
		}

		public virtual bool IsUpdate()
		{
			return _address == Update;
		}

		public static bool IsNull(Db4objects.Db4o.Internal.Slots.Slot slot)
		{
			return slot == null || slot.IsNull();
		}
	}
}
