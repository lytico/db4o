/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public abstract class BTreeTestCaseBase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		protected const int BtreeNodeSize = 4;

		protected BTree _btree;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			_btree = NewBTree();
		}

		protected virtual BTree NewBTree()
		{
			return BTreeAssert.CreateIntKeyBTree(Container(), 0, BtreeNodeSize);
		}

		protected virtual IBTreeRange Range(int lower, int upper)
		{
			IBTreeRange lowerRange = Search(lower);
			IBTreeRange upperRange = Search(upper);
			return lowerRange.ExtendToLastOf(upperRange);
		}

		protected virtual IBTreeRange Search(int key)
		{
			return Search(Trans(), key);
		}

		protected virtual void Add(int[] keys)
		{
			for (int i = 0; i < keys.Length; ++i)
			{
				Add(keys[i]);
			}
		}

		protected virtual IBTreeRange Search(Transaction trans, int key)
		{
			return _btree.SearchRange(trans, key);
		}

		protected virtual void Commit(Transaction trans)
		{
			_btree.Commit(trans);
		}

		protected override void Commit()
		{
			Commit(Trans());
		}

		protected virtual void Remove(Transaction transaction, int[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				Remove(transaction, keys[i]);
			}
		}

		protected virtual void Add(Transaction transaction, int[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				Add(transaction, keys[i]);
			}
		}

		protected virtual void AssertEmpty(Transaction transaction)
		{
			BTreeAssert.AssertEmpty(transaction, _btree);
		}

		protected virtual void Add(Transaction transaction, int element)
		{
			_btree.Add(transaction, element);
		}

		protected virtual void Remove(int element)
		{
			Remove(Trans(), element);
		}

		protected virtual void Remove(Transaction trans, int element)
		{
			_btree.Remove(trans, element);
		}

		protected virtual void Add(int element)
		{
			Add(Trans(), element);
		}

		private int Size()
		{
			return _btree.Size(Trans());
		}

		protected virtual void AssertSize(int expected)
		{
			Assert.AreEqual(expected, Size());
		}

		protected virtual void AssertSingleElement(int element)
		{
			AssertSingleElement(Trans(), element);
		}

		protected virtual void AssertSingleElement(Transaction trans, int element)
		{
			BTreeAssert.AssertSingleElement(trans, _btree, element);
		}

		protected virtual void AssertPointerKey(int key, BTreePointer pointer)
		{
			Assert.AreEqual(key, pointer.Key());
		}
	}
}
