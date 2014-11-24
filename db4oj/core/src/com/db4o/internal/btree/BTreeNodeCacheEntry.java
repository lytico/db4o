/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.internal.*;

/**
 * @exclude
 */
public class BTreeNodeCacheEntry {
	
	public final BTreeNode _node;
	
	private ByteArrayBuffer _buffer;

	public BTreeNodeCacheEntry(BTreeNode node) {
		_node = node;
	}
	
	public ByteArrayBuffer buffer() {
		return _buffer;
	}

	public void buffer(ByteArrayBuffer buffer) {
		_buffer = buffer;
	}

}
