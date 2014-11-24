/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class StatefulQueryResult : IEnumerable
	{
		private readonly IQueryResult _delegate;

		private readonly Iterable4Adaptor _iterable;

		public StatefulQueryResult(IQueryResult queryResult)
		{
			_delegate = queryResult;
			_iterable = new Iterable4Adaptor(queryResult);
		}

		public virtual object Get(int index)
		{
			lock (Lock())
			{
				return _delegate.Get(index);
			}
		}

		public virtual long[] GetIDs()
		{
			lock (Lock())
			{
				long[] ids = new long[Size()];
				int i = 0;
				IIntIterator4 iterator = _delegate.IterateIDs();
				while (iterator.MoveNext())
				{
					ids[i++] = iterator.CurrentInt();
				}
				return ids;
			}
		}

		public virtual bool HasNext()
		{
			lock (Lock())
			{
				return _iterable.HasNext();
			}
		}

		public virtual object Next()
		{
			lock (Lock())
			{
				return _iterable.Next();
			}
		}

		public virtual void Reset()
		{
			lock (Lock())
			{
				_iterable.Reset();
			}
		}

		public virtual int Size()
		{
			lock (Lock())
			{
				return _delegate.Size();
			}
		}

		public virtual void Sort(IQueryComparator cmp)
		{
			lock (Lock())
			{
				_delegate.Sort(cmp);
			}
		}

		public virtual object Lock()
		{
			return _delegate.Lock();
		}

		internal virtual IExtObjectContainer ObjectContainer()
		{
			return _delegate.ObjectContainer();
		}

		public virtual int IndexOf(object a_object)
		{
			lock (Lock())
			{
				int id = (int)ObjectContainer().GetID(a_object);
				if (id <= 0)
				{
					return -1;
				}
				return _delegate.IndexOf(id);
			}
		}

		public virtual IEnumerator IterateIDs()
		{
			lock (Lock())
			{
				return _delegate.IterateIDs();
			}
		}

		public virtual IEnumerator GetEnumerator()
		{
			lock (Lock())
			{
				return _delegate.GetEnumerator();
			}
		}

		public virtual void Skip(int count)
		{
			_delegate.Skip(count);
		}
	}
}
