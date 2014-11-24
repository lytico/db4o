/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Collections;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Collections
{
	/// <exclude></exclude>
	public partial class BigSet<E> : Db4objects.Db4o.Collections.ISet<E>, IBigSetPersistence
	{
		private Db4objects.Db4o.Internal.Btree.BTree _bTree;

		private Db4objects.Db4o.Internal.Transaction _transaction;

		public BigSet(LocalObjectContainer db)
		{
			if (db == null)
			{
				return;
			}
			_transaction = db.Transaction;
			_bTree = BTreeManager().NewBTree();
		}

		private ObjectContainerBase Container()
		{
			return Transaction().Container();
		}

		public virtual bool Add(E obj)
		{
			lock (Lock())
			{
				int id = GetID(obj);
				if (id == 0)
				{
					Add(Store(obj));
					return true;
				}
				if (Contains(id))
				{
					return false;
				}
				Add(id);
				return true;
			}
		}

		private int Store(E obj)
		{
			return Container().Store(_transaction, obj, Container().UpdateDepthProvider().Unspecified
				(NullModifiedObjectQuery.Instance));
		}

		private void Add(int id)
		{
			BTreeForUpdate().Add(_transaction, id);
		}

		private int GetID(object obj)
		{
			return (int)Container().GetID(obj);
		}

		public virtual bool AddAll(IEnumerable<E> iterable)
		{
			bool result = false;
			foreach (E element in iterable)
			{
				if (Add(element))
				{
					result = true;
				}
			}
			return result;
		}

		public virtual void Clear()
		{
			lock (Lock())
			{
				BTreeForUpdate().Clear(Transaction());
			}
		}

		public virtual bool Contains(object obj)
		{
			int id = GetID(obj);
			if (id == 0)
			{
				return false;
			}
			return Contains(id);
		}

		private bool Contains(int id)
		{
			lock (Lock())
			{
				IBTreeRange range = BTree().SearchRange(Transaction(), id);
				return !range.IsEmpty();
			}
		}

		public virtual bool IsEmpty
		{
			get
			{
				return Count == 0;
			}
		}

		private IEnumerator BTreeIterator()
		{
			return new SynchronizedIterator4(BTree().Iterator(Transaction()), Lock());
		}

		public virtual bool Remove(object obj)
		{
			lock (Lock())
			{
				if (!Contains(obj))
				{
					return false;
				}
				int id = GetID(obj);
				BTreeForUpdate().Remove(Transaction(), id);
				return true;
			}
		}

		public virtual int Count
		{
			get
			{
				lock (Lock())
				{
					return BTree().Size(Transaction());
				}
			}
		}

		public virtual object[] ToArray()
		{
			throw new NotSupportedException();
		}

		public virtual T[] ToArray<T>(T[] a)
		{
			throw new NotSupportedException();
		}

		public virtual void Write(IWriteContext context)
		{
			int id = BTree().GetID();
			if (id == 0)
			{
				BTree().Write(SystemTransaction());
			}
			context.WriteInt(BTree().GetID());
		}

		public virtual void Read(IReadContext context)
		{
			int id = context.ReadInt();
			if (_bTree != null)
			{
				AssertCurrentBTreeId(id);
				return;
			}
			_transaction = context.Transaction();
			_bTree = BTreeManager().ProduceBTree(id);
		}

		private BigSetBTreeManager BTreeManager()
		{
			return new BigSetBTreeManager(_transaction);
		}

		private void AssertCurrentBTreeId(int id)
		{
			if (id != _bTree.GetID())
			{
				throw new InvalidOperationException();
			}
		}

		private Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _transaction;
		}

		private Db4objects.Db4o.Internal.Transaction SystemTransaction()
		{
			return Container().SystemTransaction();
		}

		public virtual void Invalidate()
		{
			_bTree = null;
		}

		private Db4objects.Db4o.Internal.Btree.BTree BTree()
		{
			if (_bTree == null)
			{
				throw new InvalidOperationException();
			}
			return _bTree;
		}

		private Db4objects.Db4o.Internal.Btree.BTree BTreeForUpdate()
		{
			Db4objects.Db4o.Internal.Btree.BTree bTree = BTree();
			BTreeManager().EnsureIsManaged(bTree);
			return bTree;
		}

		private object Element(int id)
		{
			object obj = Container().GetByID(Transaction(), id);
			Container().Activate(obj);
			return obj;
		}

		private object Lock()
		{
			return Container().Lock();
		}
	}
}
