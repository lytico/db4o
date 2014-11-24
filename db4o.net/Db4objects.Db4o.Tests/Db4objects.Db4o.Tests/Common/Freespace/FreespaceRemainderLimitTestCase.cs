/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class FreespaceRemainderLimitTestCase : ITestCase
	{
		private const int BlockSize = 7;

		private readonly IBlockConverter _blockConverter = new BlockSizeBlockConverter(BlockSize
			);

		private readonly InMemoryFreespaceManager _blocked = new InMemoryFreespaceManager
			(null, 0, 2);

		private readonly BlockAwareFreespaceManager _nonBlocked;

		public virtual void TestAllocateSlotWithRemainder()
		{
			AssertAllocateSlot(3 * BlockSize, BlockSize + 1, 3 * BlockSize);
		}

		public virtual void TestAllocateSlotNoRemainder()
		{
			AssertAllocateSlot(5 * BlockSize, BlockSize + 1, 2 * BlockSize);
		}

		private void AssertAllocateSlot(int freeSlotSize, int requiredSlotSize, int expectedSlotSize
			)
		{
			Slot slot = new Slot(1, freeSlotSize);
			_nonBlocked.Free(slot);
			Slot allocatedSlot = _nonBlocked.AllocateSlot(requiredSlotSize);
			int expectedAllocatedSlotLength = _blockConverter.BlockAlignedBytes(expectedSlotSize
				);
			Slot expectedSlot = new Slot(1, expectedAllocatedSlotLength);
			Assert.AreEqual(expectedSlot, allocatedSlot);
		}

		public FreespaceRemainderLimitTestCase()
		{
			_nonBlocked = new BlockAwareFreespaceManager(_blocked, new BlockSizeBlockConverter
				(BlockSize));
		}
	}
}
