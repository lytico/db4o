/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	public abstract class BTreeUpdate : BTreePatch
	{
		protected Db4objects.Db4o.Internal.Btree.BTreeUpdate _next;

		public BTreeUpdate(Transaction transaction, object obj) : base(transaction, obj)
		{
		}

		protected virtual bool HasNext()
		{
			return _next != null;
		}

		public override BTreePatch ForTransaction(Transaction trans)
		{
			if (_transaction == trans)
			{
				return this;
			}
			if (_next == null)
			{
				return null;
			}
			return _next.ForTransaction(trans);
		}

		public virtual Db4objects.Db4o.Internal.Btree.BTreeUpdate RemoveFor(Transaction trans
			)
		{
			if (_transaction == trans)
			{
				return _next;
			}
			if (_next != null)
			{
				_next = _next.RemoveFor(trans);
			}
			return this;
		}

		public virtual void Append(Db4objects.Db4o.Internal.Btree.BTreeUpdate patch)
		{
			if (_transaction == patch._transaction)
			{
				throw new ArgumentException();
			}
			if (!HasNext())
			{
				_next = patch;
			}
			else
			{
				_next.Append(patch);
			}
		}

		protected virtual void ApplyKeyChange(object obj)
		{
			_object = obj;
			if (HasNext())
			{
				_next.ApplyKeyChange(obj);
			}
		}

		protected abstract void Committed(BTree btree);

		public override object Commit(Transaction trans, BTree btree, BTreeNode node)
		{
			Db4objects.Db4o.Internal.Btree.BTreeUpdate patch = (Db4objects.Db4o.Internal.Btree.BTreeUpdate
				)ForTransaction(trans);
			if (patch is BTreeCancelledRemoval)
			{
				object obj = patch.GetCommittedObject();
				ApplyKeyChange(obj);
			}
			else
			{
				if (patch is BTreeRemove)
				{
					RemovedBy(trans, btree, node);
					patch.Committed(btree);
					return No4.Instance;
				}
			}
			return InternalCommit(trans, btree);
		}

		protected object InternalCommit(Transaction trans, BTree btree)
		{
			if (_transaction == trans)
			{
				Committed(btree);
				if (HasNext())
				{
					return _next;
				}
				return GetCommittedObject();
			}
			if (HasNext())
			{
				SetNextIfPatch(_next.InternalCommit(trans, btree));
			}
			return this;
		}

		private void SetNextIfPatch(object newNext)
		{
			if (newNext is Db4objects.Db4o.Internal.Btree.BTreeUpdate)
			{
				_next = (Db4objects.Db4o.Internal.Btree.BTreeUpdate)newNext;
			}
			else
			{
				_next = null;
			}
		}

		protected abstract object GetCommittedObject();

		public override object Rollback(Transaction trans, BTree btree)
		{
			if (_transaction == trans)
			{
				if (HasNext())
				{
					return _next;
				}
				return GetObject();
			}
			if (HasNext())
			{
				SetNextIfPatch(_next.Rollback(trans, btree));
			}
			return this;
		}

		public override object Key(Transaction trans)
		{
			BTreePatch patch = ForTransaction(trans);
			if (patch == null)
			{
				return GetObject();
			}
			if (patch.IsRemove())
			{
				return No4.Instance;
			}
			return patch.GetObject();
		}

		public virtual Db4objects.Db4o.Internal.Btree.BTreeUpdate ReplacePatch(BTreePatch
			 patch, Db4objects.Db4o.Internal.Btree.BTreeUpdate update)
		{
			if (patch == this)
			{
				update._next = _next;
				return update;
			}
			if (_next == null)
			{
				throw new InvalidOperationException();
			}
			_next = _next.ReplacePatch(patch, update);
			return this;
		}

		public virtual void RemovedBy(Transaction trans, BTree btree, BTreeNode node)
		{
			if (trans != _transaction)
			{
				AdjustSizeOnRemovalByOtherTransaction(btree, node);
			}
			if (HasNext())
			{
				_next.RemovedBy(trans, btree, node);
			}
		}

		protected abstract void AdjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode
			 node);

		public override int SizeDiff(Transaction trans)
		{
			Db4objects.Db4o.Internal.Btree.BTreeUpdate patchForTransaction = (Db4objects.Db4o.Internal.Btree.BTreeUpdate
				)ForTransaction(trans);
			if (patchForTransaction == null)
			{
				return 1;
			}
			return patchForTransaction.SizeDiff();
		}

		protected abstract int SizeDiff();
	}
}
