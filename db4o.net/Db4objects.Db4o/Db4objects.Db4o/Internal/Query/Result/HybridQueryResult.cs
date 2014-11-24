/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class HybridQueryResult : AbstractQueryResult
	{
		private AbstractQueryResult _delegate;

		public HybridQueryResult(Transaction transaction, QueryEvaluationMode mode) : base
			(transaction)
		{
			_delegate = ForMode(transaction, mode);
		}

		private static AbstractQueryResult ForMode(Transaction transaction, QueryEvaluationMode
			 mode)
		{
			if (mode == QueryEvaluationMode.Lazy)
			{
				return new LazyQueryResult(transaction);
			}
			if (mode == QueryEvaluationMode.Snapshot)
			{
				return new SnapShotQueryResult(transaction);
			}
			return new IdListQueryResult(transaction);
		}

		public override object Get(int index)
		{
			_delegate = _delegate.SupportElementAccess();
			return _delegate.Get(index);
		}

		public override int GetId(int index)
		{
			_delegate = _delegate.SupportElementAccess();
			return _delegate.GetId(index);
		}

		public override int IndexOf(int id)
		{
			_delegate = _delegate.SupportElementAccess();
			return _delegate.IndexOf(id);
		}

		public override IIntIterator4 IterateIDs()
		{
			return _delegate.IterateIDs();
		}

		public override IEnumerator GetEnumerator()
		{
			return _delegate.GetEnumerator();
		}

		public override void LoadFromClassIndex(ClassMetadata clazz)
		{
			_delegate.LoadFromClassIndex(clazz);
		}

		public override void LoadFromClassIndexes(ClassMetadataIterator iterator)
		{
			_delegate.LoadFromClassIndexes(iterator);
		}

		public override void LoadFromIdReader(IEnumerator reader)
		{
			_delegate.LoadFromIdReader(reader);
		}

		public override void LoadFromQuery(QQuery query)
		{
			if (query.RequiresSort())
			{
				_delegate = new IdListQueryResult(Transaction());
			}
			_delegate.LoadFromQuery(query);
		}

		public override int Size()
		{
			_delegate = _delegate.SupportSize();
			return _delegate.Size();
		}

		public override void Sort(IQueryComparator cmp)
		{
			_delegate = _delegate.SupportSort();
			_delegate.Sort(cmp);
		}

		public override void SortIds(IIntComparator cmp)
		{
			_delegate = _delegate.SupportSort();
			_delegate.SortIds(cmp);
		}

		public override void Skip(int count)
		{
			_delegate.Skip(count);
		}
	}
}
