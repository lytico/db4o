/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Cluster;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Cluster
{
	/// <exclude></exclude>
	public class ClusterQueryResult : IQueryResult
	{
		private readonly Db4objects.Db4o.Cluster.Cluster _cluster;

		private readonly IObjectSet[] _objectSets;

		private readonly int[] _sizes;

		private readonly int _size;

		public ClusterQueryResult(Db4objects.Db4o.Cluster.Cluster cluster, IQuery[] queries
			)
		{
			_cluster = cluster;
			_objectSets = new IObjectSet[queries.Length];
			_sizes = new int[queries.Length];
			int size = 0;
			for (int i = 0; i < queries.Length; i++)
			{
				_objectSets[i] = queries[i].Execute();
				_sizes[i] = _objectSets[i].Count;
				size += _sizes[i];
			}
			_size = size;
		}

		private sealed class ClusterQueryResultIntIterator : IIntIterator4
		{
			private readonly CompositeIterator4 _delegate;

			public ClusterQueryResultIntIterator(IEnumerator[] iterators)
			{
				_delegate = new CompositeIterator4(iterators);
			}

			public bool MoveNext()
			{
				return _delegate.MoveNext();
			}

			public object Current
			{
				get
				{
					return _delegate.Current;
				}
			}

			public void Reset()
			{
				_delegate.Reset();
			}

			public int CurrentInt()
			{
				return ((IIntIterator4)_delegate.CurrentIterator()).CurrentInt();
			}
		}

		public virtual IIntIterator4 IterateIDs()
		{
			lock (_cluster)
			{
				IEnumerator[] iterators = new IEnumerator[_objectSets.Length];
				for (int i = 0; i < _objectSets.Length; i++)
				{
					iterators[i] = ((ObjectSetFacade)_objectSets[i])._delegate.IterateIDs();
				}
				return new ClusterQueryResult.ClusterQueryResultIntIterator(iterators);
			}
		}

		public virtual IEnumerator GetEnumerator()
		{
			lock (_cluster)
			{
				IEnumerator[] iterators = new IEnumerator[_objectSets.Length];
				for (int i = 0; i < _objectSets.Length; i++)
				{
					iterators[i] = ((ObjectSetFacade)_objectSets[i])._delegate.GetEnumerator();
				}
				return new CompositeIterator4(iterators);
			}
		}

		public virtual int Size()
		{
			return _size;
		}

		public virtual object Get(int index)
		{
			lock (_cluster)
			{
				if (index < 0 || index >= Size())
				{
					throw new IndexOutOfRangeException();
				}
				int i = 0;
				while (index >= _sizes[i])
				{
					index -= _sizes[i];
					i++;
				}
				return ((ObjectSetFacade)_objectSets[i])[index];
			}
		}

		public virtual object Lock()
		{
			return _cluster;
		}

		public virtual IExtObjectContainer ObjectContainer()
		{
			throw new NotSupportedException();
		}

		public virtual int IndexOf(int id)
		{
			throw new NotSupportedException();
		}

		public virtual void Sort(IQueryComparator cmp)
		{
			throw new NotSupportedException();
		}

		public virtual void SortIds(IIntComparator cmp)
		{
			throw new NotSupportedException();
		}

		public virtual void LoadFromClassIndex(ClassMetadata c)
		{
			throw new NotSupportedException();
		}

		public virtual void LoadFromQuery(QQuery q)
		{
			throw new NotSupportedException();
		}

		public virtual void LoadFromClassIndexes(ClassMetadataIterator i)
		{
			throw new NotSupportedException();
		}

		public virtual void LoadFromIdReader(ByteArrayBuffer r)
		{
			throw new NotSupportedException();
		}

		public virtual void Skip(int count)
		{
			throw new NotImplementedException();
		}
	}
}
