/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public abstract class JoinedLeaf : IIndexedNodeWithRange
	{
		private readonly QCon _constraint;

		private readonly IIndexedNodeWithRange _leaf1;

		private readonly IBTreeRange _range;

		public JoinedLeaf(QCon constraint, IIndexedNodeWithRange leaf1, IBTreeRange range
			)
		{
			if (null == constraint || null == leaf1 || null == range)
			{
				throw new ArgumentNullException();
			}
			_constraint = constraint;
			_leaf1 = leaf1;
			_range = range;
		}

		public virtual QCon GetConstraint()
		{
			return _constraint;
		}

		public virtual IBTreeRange GetRange()
		{
			return _range;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _range.Keys();
		}

		public virtual BTree GetIndex()
		{
			return _leaf1.GetIndex();
		}

		public virtual bool IsResolved()
		{
			return _leaf1.IsResolved();
		}

		public virtual IIndexedNode Resolve()
		{
			return IndexedPath.NewParentPath(this, _constraint);
		}

		public virtual int ResultSize()
		{
			return _range.Size();
		}

		public virtual bool IsEmpty()
		{
			return _range.IsEmpty();
		}

		public virtual void MarkAsBestIndex(QCandidates candidates)
		{
			_leaf1.MarkAsBestIndex(candidates);
			_constraint.SetProcessedByIndex(candidates);
		}

		public virtual void Traverse(IIntVisitor visitor)
		{
			IndexedNodeBase.Traverse(this, visitor);
		}
	}
}
