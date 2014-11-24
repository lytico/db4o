/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class DisabledBlockConverter : IBlockConverter
	{
		public virtual int BlockAlignedBytes(int bytes)
		{
			return bytes;
		}

		public virtual int BlocksToBytes(int blocks)
		{
			return blocks;
		}

		public virtual int BytesToBlocks(long bytes)
		{
			return (int)bytes;
		}

		public virtual Slot ToBlockedLength(Slot slot)
		{
			return slot;
		}

		public virtual Slot ToNonBlockedLength(Slot slot)
		{
			return slot;
		}
	}
}
