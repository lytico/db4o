/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class SlotChangeFactory
	{
		private SlotChangeFactory()
		{
		}

		public virtual SlotChange NewInstance(int id)
		{
			return new SlotChange(id);
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory UserObjects
			 = new Db4objects.Db4o.Internal.Slots.SlotChangeFactory();

		private sealed class _SlotChangeFactory_20 : Db4objects.Db4o.Internal.Slots.SlotChangeFactory
		{
			public _SlotChangeFactory_20()
			{
			}

			public override SlotChange NewInstance(int id)
			{
				return new SystemSlotChange(id);
			}
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory SystemObjects
			 = new _SlotChangeFactory_20();

		private sealed class _SlotChangeFactory_26 : Db4objects.Db4o.Internal.Slots.SlotChangeFactory
		{
			public _SlotChangeFactory_26()
			{
			}

			public override SlotChange NewInstance(int id)
			{
				return new IdSystemSlotChange(id);
			}
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory IdSystem = 
			new _SlotChangeFactory_26();

		private sealed class _SlotChangeFactory_32 : Db4objects.Db4o.Internal.Slots.SlotChangeFactory
		{
			public _SlotChangeFactory_32()
			{
			}

			public override SlotChange NewInstance(int id)
			{
				return new FreespaceSlotChange(id);
			}
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory FreeSpace
			 = new _SlotChangeFactory_32();
	}
}
