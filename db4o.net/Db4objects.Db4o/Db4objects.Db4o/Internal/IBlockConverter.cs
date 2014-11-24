/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public interface IBlockConverter
	{
		int BytesToBlocks(long bytes);

		int BlockAlignedBytes(int bytes);

		int BlocksToBytes(int blocks);

		Slot ToBlockedLength(Slot slot);

		Slot ToNonBlockedLength(Slot slot);
	}
}
