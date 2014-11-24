/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	/// <exclude></exclude>
	public class BlockConverterTestCase : ITestCase
	{
		private readonly BlockSizeBlockConverter blockConverter = new BlockSizeBlockConverter
			(8);

		public virtual void TestBlocksToBytes()
		{
			int[] blocks = new int[] { 0, 1, 8, 9 };
			int[] bytes = new int[] { 0, 8, 64, 72 };
			for (int i = 0; i < blocks.Length; i++)
			{
				Assert.AreEqual(bytes[i], blockConverter.BlocksToBytes(blocks[i]));
			}
		}

		public virtual void TestBytesToBlocks()
		{
			int[] bytes = new int[] { 0, 1, 2, 7, 8, 9, 16, 17, 799, 800, 801 };
			int[] blocks = new int[] { 0, 1, 1, 1, 1, 2, 2, 3, 100, 100, 101 };
			for (int i = 0; i < blocks.Length; i++)
			{
				Assert.AreEqual(blocks[i], blockConverter.BytesToBlocks(bytes[i]));
			}
		}

		public virtual void TestBlockAlignedBytes()
		{
			int[] bytes = new int[] { 0, 1, 2, 7, 8, 9, 16, 17, 799, 800, 801 };
			int[] aligned = new int[] { 0, 8, 8, 8, 8, 16, 16, 24, 800, 800, 808 };
			for (int i = 0; i < bytes.Length; i++)
			{
				Assert.AreEqual(aligned[i], blockConverter.BlockAlignedBytes(bytes[i]));
			}
		}

		public virtual void TestToBlockedLength()
		{
			int[] bytes = new int[] { 0, 1, 2, 7, 8, 9, 16, 17, 799, 800, 801 };
			int[] blocks = new int[] { 0, 1, 1, 1, 1, 2, 2, 3, 100, 100, 101 };
			for (int i = 0; i < bytes.Length; i++)
			{
				Slot byteSlot = new Slot(0, bytes[i]);
				Slot blockedSlot = new Slot(0, blocks[i]);
				Assert.AreEqual(blockedSlot, blockConverter.ToBlockedLength(byteSlot));
			}
		}

		public virtual void TestToNonBlockedLength()
		{
			int[] blocks = new int[] { 0, 1, 8, 9 };
			int[] bytes = new int[] { 0, 8, 64, 72 };
			for (int i = 0; i < blocks.Length; i++)
			{
				Slot blockedSlot = new Slot(0, blocks[i]);
				Slot byteSlot = new Slot(0, bytes[i]);
				Assert.AreEqual(byteSlot, blockConverter.ToNonBlockedLength(blockedSlot));
			}
		}
	}
}
