/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class BlockAwareFreespaceManagerTestCase : ITestCase
	{
		private const int BlockSize = 7;

		private readonly IBlockConverter _blockConverter = new BlockSizeBlockConverter(BlockSize
			);

		private readonly InMemoryFreespaceManager _blocked = new InMemoryFreespaceManager
			(null, 0, 0);

		private readonly BlockAwareFreespaceManager _nonBlocked;

		public virtual void TestFree()
		{
			Slot slot = new Slot(1, 15);
			_nonBlocked.Free(slot);
			AssertContains(new Slot[] { _blockConverter.ToBlockedLength(slot) });
		}

		public virtual void TestAllocateSlot()
		{
			Slot slot = new Slot(1, 15);
			_nonBlocked.Free(slot);
			Slot allocatedSlot = _nonBlocked.AllocateSlot(8);
			int expectedAllocatedSlotLength = _blockConverter.BlockAlignedBytes(8);
			Slot expectedSlot = new Slot(1, expectedAllocatedSlotLength);
			Assert.AreEqual(expectedSlot, allocatedSlot);
		}

		public virtual void TestNoSlotAvailable()
		{
			Slot slot = _nonBlocked.AllocateSlot(1);
			Assert.IsNull(slot);
		}

		private void AssertContains(Slot[] slots)
		{
			IList foundSlots = new ArrayList();
			_blocked.Traverse(new _IVisitor4_47(foundSlots));
			IteratorAssert.SameContent(slots, foundSlots);
		}

		private sealed class _IVisitor4_47 : IVisitor4
		{
			public _IVisitor4_47(IList foundSlots)
			{
				this.foundSlots = foundSlots;
			}

			public void Visit(object slot)
			{
				foundSlots.Add(((Slot)slot));
			}

			private readonly IList foundSlots;
		}

		public BlockAwareFreespaceManagerTestCase()
		{
			_nonBlocked = new BlockAwareFreespaceManager(_blocked, new BlockSizeBlockConverter
				(BlockSize));
		}
	}
}
