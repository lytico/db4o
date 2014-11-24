/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeNodeSearchResult
	{
		private readonly Transaction _transaction;

		private readonly BTree _btree;

		private readonly BTreePointer _pointer;

		private readonly bool _foundMatch;

		internal BTreeNodeSearchResult(Transaction transaction, BTree btree, BTreePointer
			 pointer, bool foundMatch)
		{
			if (null == transaction || null == btree)
			{
				throw new ArgumentNullException();
			}
			_transaction = transaction;
			_btree = btree;
			_pointer = pointer;
			_foundMatch = foundMatch;
		}

		internal BTreeNodeSearchResult(Transaction trans, ByteArrayBuffer nodeReader, BTree
			 btree, BTreeNode node, int cursor, bool foundMatch) : this(trans, btree, PointerOrNull
			(trans, nodeReader, node, cursor), foundMatch)
		{
		}

		internal BTreeNodeSearchResult(Transaction trans, ByteArrayBuffer nodeReader, BTree
			 btree, Searcher searcher, BTreeNode node) : this(trans, btree, NextPointerIf(PointerOrNull
			(trans, nodeReader, node, searcher.Cursor()), searcher.IsGreater()), searcher.FoundMatch
			())
		{
		}

		private static BTreePointer NextPointerIf(BTreePointer pointer, bool condition)
		{
			if (null == pointer)
			{
				return null;
			}
			if (condition)
			{
				return pointer.Next();
			}
			return pointer;
		}

		private static BTreePointer PointerOrNull(Transaction trans, ByteArrayBuffer nodeReader
			, BTreeNode node, int cursor)
		{
			return node == null ? null : new BTreePointer(trans, nodeReader, node, cursor);
		}

		public virtual IBTreeRange CreateIncludingRange(Db4objects.Db4o.Internal.Btree.BTreeNodeSearchResult
			 end)
		{
			BTreePointer firstPointer = FirstValidPointer();
			BTreePointer endPointer = end._foundMatch ? end._pointer.Next() : end.FirstValidPointer
				();
			return new BTreeRangeSingle(_transaction, _btree, firstPointer, endPointer);
		}

		public virtual BTreePointer FirstValidPointer()
		{
			if (null == _pointer)
			{
				return null;
			}
			if (_pointer.IsValid())
			{
				return _pointer;
			}
			return _pointer.Next();
		}
	}
}
