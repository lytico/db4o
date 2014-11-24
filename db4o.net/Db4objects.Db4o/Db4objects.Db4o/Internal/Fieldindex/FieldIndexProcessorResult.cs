/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public class FieldIndexProcessorResult : IIntVisitable
	{
		public static readonly Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			 NoIndexFound = new Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			(null);

		public static readonly Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			 FoundIndexButNoMatch = new Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			(null);

		private readonly IIndexedNode _indexedNode;

		public FieldIndexProcessorResult(IIndexedNode indexedNode)
		{
			_indexedNode = indexedNode;
		}

		public virtual void Traverse(IIntVisitor visitor)
		{
			if (!FoundMatch())
			{
				return;
			}
			_indexedNode.Traverse(visitor);
		}

		public virtual bool FoundMatch()
		{
			return FoundIndex() && !NoMatch();
		}

		public virtual bool FoundIndex()
		{
			return this != NoIndexFound;
		}

		public virtual bool NoMatch()
		{
			return this == FoundIndexButNoMatch;
		}

		public virtual IEnumerator IterateIDs()
		{
			return new _MappingIterator_40(_indexedNode.GetEnumerator());
		}

		private sealed class _MappingIterator_40 : MappingIterator
		{
			public _MappingIterator_40(IEnumerator baseArg1) : base(baseArg1)
			{
			}

			protected override object Map(object current)
			{
				IFieldIndexKey composite = (IFieldIndexKey)current;
				return composite.ParentID();
			}
		}
	}
}
