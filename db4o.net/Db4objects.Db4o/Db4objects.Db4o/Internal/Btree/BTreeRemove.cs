/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeRemove : BTreeUpdate
	{
		public BTreeRemove(Transaction transaction, object obj) : base(transaction, obj)
		{
		}

		protected override void Committed(BTree btree)
		{
			btree.NotifyRemoveListener(new TransactionContext(_transaction, GetObject()));
		}

		public override string ToString()
		{
			return "(-) " + base.ToString();
		}

		public override bool IsRemove()
		{
			return true;
		}

		protected override object GetCommittedObject()
		{
			return No4.Instance;
		}

		protected override void AdjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode
			 node)
		{
			// The size was reduced for this entry, let's change back.
			btree.SizeChanged(_transaction, node, +1);
		}

		protected override int SizeDiff()
		{
			return 0;
		}
	}
}
