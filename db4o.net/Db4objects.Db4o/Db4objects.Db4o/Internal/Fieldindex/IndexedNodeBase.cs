/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public abstract class IndexedNodeBase : IIndexedNode
	{
		protected readonly QConObject _constraint;

		public IndexedNodeBase(QConObject qcon)
		{
			if (null == qcon)
			{
				throw new ArgumentNullException();
			}
			if (null == qcon.GetField())
			{
				throw new ArgumentException();
			}
			_constraint = qcon;
		}

		public BTree GetIndex()
		{
			return FieldMetadata().GetIndex(Transaction());
		}

		private Db4objects.Db4o.Internal.FieldMetadata FieldMetadata()
		{
			return _constraint.GetField().GetFieldMetadata();
		}

		public virtual QCon Constraint()
		{
			return _constraint;
		}

		public virtual bool IsResolved()
		{
			QCon parent = Constraint().Parent();
			return null == parent || !parent.HasParent();
		}

		public virtual IBTreeRange Search(object value)
		{
			return FieldMetadata().Search(Transaction(), value);
		}

		public static void Traverse(IIndexedNode node, IIntVisitor visitor)
		{
			IEnumerator i = node.GetEnumerator();
			while (i.MoveNext())
			{
				IFieldIndexKey composite = (IFieldIndexKey)i.Current;
				visitor.Visit(composite.ParentID());
			}
		}

		public virtual void Traverse(IIntVisitor visitor)
		{
			Traverse(this, visitor);
		}

		public virtual IIndexedNode Resolve()
		{
			if (IsResolved())
			{
				return null;
			}
			return IndexedPath.NewParentPath(this, Constraint());
		}

		private Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return Constraint().Transaction();
		}

		public override string ToString()
		{
			return "IndexedNode " + _constraint.ToString();
		}

		public abstract IEnumerator GetEnumerator();

		public abstract bool IsEmpty();

		public abstract void MarkAsBestIndex(QCandidates arg1);

		public abstract int ResultSize();
	}
}
