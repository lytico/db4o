/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeNodeCacheEntry
	{
		public readonly BTreeNode _node;

		private ByteArrayBuffer _buffer;

		public BTreeNodeCacheEntry(BTreeNode node)
		{
			_node = node;
		}

		public virtual ByteArrayBuffer Buffer()
		{
			return _buffer;
		}

		public virtual void Buffer(ByteArrayBuffer buffer)
		{
			_buffer = buffer;
		}
	}
}
