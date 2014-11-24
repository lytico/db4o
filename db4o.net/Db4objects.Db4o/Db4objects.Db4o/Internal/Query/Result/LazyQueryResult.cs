/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class LazyQueryResult : AbstractLateQueryResult
	{
		public LazyQueryResult(Transaction trans) : base(trans)
		{
		}

		public override void LoadFromClassIndex(ClassMetadata clazz)
		{
			_iterable = ClassIndexIterable(clazz);
		}

		public override void LoadFromClassIndexes(ClassMetadataIterator classCollectionIterator
			)
		{
			_iterable = ClassIndexesIterable(classCollectionIterator);
		}

		public override void LoadFromQuery(QQuery query)
		{
			_iterable = new _IEnumerable_28(query);
		}

		private sealed class _IEnumerable_28 : IEnumerable
		{
			public _IEnumerable_28(QQuery query)
			{
				this.query = query;
			}

			public IEnumerator GetEnumerator()
			{
				return query.ExecuteLazy();
			}

			private readonly QQuery query;
		}
	}
}
