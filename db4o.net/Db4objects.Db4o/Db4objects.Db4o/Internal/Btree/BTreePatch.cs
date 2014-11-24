/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	public abstract class BTreePatch
	{
		protected readonly Transaction _transaction;

		protected object _object;

		public BTreePatch(Transaction transaction, object obj)
		{
			_transaction = transaction;
			_object = obj;
		}

		public abstract object Commit(Transaction trans, BTree btree, BTreeNode node);

		public abstract Db4objects.Db4o.Internal.Btree.BTreePatch ForTransaction(Transaction
			 trans);

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual bool IsAdd()
		{
			return false;
		}

		public virtual bool IsCancelledRemoval()
		{
			return false;
		}

		public virtual bool IsRemove()
		{
			return false;
		}

		public abstract object Key(Transaction trans);

		public abstract object Rollback(Transaction trans, BTree btree);

		public override string ToString()
		{
			if (_object == null)
			{
				return "[NULL]";
			}
			return _object.ToString();
		}

		public abstract int SizeDiff(Transaction trans);
	}
}
