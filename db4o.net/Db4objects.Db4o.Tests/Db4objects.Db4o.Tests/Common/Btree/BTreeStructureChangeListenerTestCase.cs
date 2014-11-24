/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	/// <exclude></exclude>
	public class BTreeStructureChangeListenerTestCase : BTreeTestCaseBase
	{
		public virtual void TestSplits()
		{
			BooleanByRef splitNotified = new BooleanByRef();
			IBTreeStructureListener listener = new _IBTreeStructureListener_18(splitNotified);
			_btree.StructureListener(listener);
			for (int i = 0; i < BtreeNodeSize + 1; i++)
			{
				Add(i);
			}
			Assert.IsTrue(splitNotified.value);
		}

		private sealed class _IBTreeStructureListener_18 : IBTreeStructureListener
		{
			public _IBTreeStructureListener_18(BooleanByRef splitNotified)
			{
				this.splitNotified = splitNotified;
			}

			public void NotifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode
				)
			{
				Assert.IsFalse(splitNotified.value);
				splitNotified.value = true;
			}

			public void NotifyDeleted(Transaction trans, BTreeNode node)
			{
			}

			public void NotifyCountChanged(Transaction trans, BTreeNode node, int diff)
			{
			}

			private readonly BooleanByRef splitNotified;
		}

		public virtual void TestDelete()
		{
			IntByRef deletedCount = new IntByRef();
			IBTreeStructureListener listener = new _IBTreeStructureListener_39(deletedCount);
			_btree.StructureListener(listener);
			for (int i = 0; i < BtreeNodeSize + 1; i++)
			{
				Add(i);
			}
			for (int i = 0; i < BtreeNodeSize + 1; i++)
			{
				Remove(i);
			}
			Assert.AreEqual(2, deletedCount.value);
		}

		private sealed class _IBTreeStructureListener_39 : IBTreeStructureListener
		{
			public _IBTreeStructureListener_39(IntByRef deletedCount)
			{
				this.deletedCount = deletedCount;
			}

			public void NotifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode
				)
			{
			}

			public void NotifyDeleted(Transaction trans, BTreeNode node)
			{
				deletedCount.value++;
			}

			public void NotifyCountChanged(Transaction trans, BTreeNode node, int diff)
			{
			}

			private readonly IntByRef deletedCount;
		}

		public virtual void TestItemCountChanged()
		{
			IntByRef changedCount = new IntByRef();
			IBTreeStructureListener listener = new _IBTreeStructureListener_63(changedCount);
			_btree.StructureListener(listener);
			changedCount.value = 0;
			Add(42);
			Assert.AreEqual(1, changedCount.value);
			Remove(42);
			Assert.AreEqual(-1, changedCount.value);
			changedCount.value = 0;
			Remove(42);
			Assert.AreEqual(0, changedCount.value);
		}

		private sealed class _IBTreeStructureListener_63 : IBTreeStructureListener
		{
			public _IBTreeStructureListener_63(IntByRef changedCount)
			{
				this.changedCount = changedCount;
			}

			public void NotifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode
				)
			{
			}

			public void NotifyDeleted(Transaction trans, BTreeNode node)
			{
			}

			public void NotifyCountChanged(Transaction trans, BTreeNode node, int diff)
			{
				changedCount.value = diff;
			}

			private readonly IntByRef changedCount;
		}
	}
}
