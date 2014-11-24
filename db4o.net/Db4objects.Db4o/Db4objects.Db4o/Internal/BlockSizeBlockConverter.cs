/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public sealed class BlockSizeBlockConverter : IBlockConverter
	{
		private readonly int _blockSize;

		public BlockSizeBlockConverter(int blockSize)
		{
			_blockSize = blockSize;
		}

		public int BytesToBlocks(long bytes)
		{
			return (int)((bytes + _blockSize - 1) / _blockSize);
		}

		public int BlockAlignedBytes(int bytes)
		{
			return BytesToBlocks(bytes) * _blockSize;
		}

		public int BlocksToBytes(int blocks)
		{
			return blocks * _blockSize;
		}

		public Slot ToBlockedLength(Slot slot)
		{
			return new Slot(slot.Address(), BytesToBlocks(slot.Length()));
		}

		public Slot ToNonBlockedLength(Slot slot)
		{
			return new Slot(slot.Address(), BlocksToBytes(slot.Length()));
		}
	}
}
