/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeAdd : BTreePatch
	{
		public BTreeAdd(Transaction transaction, object obj) : base(transaction, obj)
		{
		}

		protected virtual object RolledBack(BTree btree)
		{
			btree.NotifyRemoveListener(new TransactionContext(_transaction, GetObject()));
			return No4.Instance;
		}

		public override string ToString()
		{
			return "(+) " + base.ToString();
		}

		public override object Commit(Transaction trans, BTree btree, BTreeNode node)
		{
			if (_transaction == trans)
			{
				return GetObject();
			}
			return this;
		}

		public override BTreePatch ForTransaction(Transaction trans)
		{
			if (_transaction == trans)
			{
				return this;
			}
			return null;
		}

		public override object Key(Transaction trans)
		{
			if (_transaction != trans)
			{
				return No4.Instance;
			}
			return GetObject();
		}

		public override object Rollback(Transaction trans, BTree btree)
		{
			if (_transaction == trans)
			{
				return RolledBack(btree);
			}
			return this;
		}

		public override bool IsAdd()
		{
			return true;
		}

		public override int SizeDiff(Transaction trans)
		{
			return _transaction == trans ? 1 : 0;
		}
	}
}
