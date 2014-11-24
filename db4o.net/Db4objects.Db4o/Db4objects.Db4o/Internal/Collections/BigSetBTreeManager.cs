/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Handlers;

namespace Db4objects.Db4o.Internal.Collections
{
	/// <exclude></exclude>
	public class BigSetBTreeManager
	{
		private sealed class _TransactionLocal_14 : TransactionLocal
		{
			public _TransactionLocal_14()
			{
			}

			public override object InitialValueFor(Transaction transaction)
			{
				return new Hashtable();
			}
		}

		private static readonly TransactionLocal _bTreesInTransaction = new _TransactionLocal_14
			();

		private readonly Transaction _transaction;

		internal BigSetBTreeManager(Transaction transaction)
		{
			_transaction = transaction;
		}

		internal virtual BTree ProduceBTree(int id)
		{
			AssertValidBTreeId(id);
			BTree bTree = ExistingBTreeInTransactionWith(id);
			if (null == bTree)
			{
				bTree = NewBTreeWithId(id);
				RegisterBTreeInTransaction(bTree);
			}
			return bTree;
		}

		internal virtual BTree NewBTree()
		{
			BTree bTree = NewBTreeWithId(0);
			bTree.Write(SystemTransaction());
			RegisterBTreeInTransaction(bTree);
			return bTree;
		}

		internal virtual void EnsureIsManaged(BTree tree)
		{
			RegisterBTreeInTransaction(tree);
		}

		private BTree NewBTreeWithId(int id)
		{
			return NewBTreeWithId(id, SystemTransaction());
		}

		private Transaction SystemTransaction()
		{
			return _transaction.SystemTransaction();
		}

		private static BTree NewBTreeWithId(int id, Transaction systemTransaction)
		{
			return new BTree(systemTransaction, id, new IntHandler());
		}

		private static void AssertValidBTreeId(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException();
			}
		}

		private void RegisterBTreeInTransaction(BTree tree)
		{
			AssertValidBTreeId(tree.GetID());
			BTreesIn(_transaction)[tree.GetID()] = tree;
		}

		private BTree ExistingBTreeInTransactionWith(int id)
		{
			return ((BTree)BTreesIn(_transaction)[id]);
		}

		private static IDictionary BTreesIn(Transaction transaction)
		{
			return ((IDictionary)transaction.Get(_bTreesInTransaction).value);
		}
	}
}
