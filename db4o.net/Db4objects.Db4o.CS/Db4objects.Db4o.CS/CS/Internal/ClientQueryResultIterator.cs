/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Result;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	internal class ClientQueryResultIterator : IEnumerator
	{
		private static readonly IPrefetchingStrategy _prefetchingStrategy = SingleMessagePrefetchingStrategy
			.Instance;

		private object[] _prefetchedObjects;

		private int _remainingObjects;

		private int _prefetchRight;

		private readonly AbstractQueryResult _client;

		private readonly IIntIterator4 _ids;

		internal ClientQueryResultIterator(AbstractQueryResult client)
		{
			_client = client;
			_ids = client.IterateIDs();
		}

		public virtual object Current
		{
			get
			{
				lock (StreamLock())
				{
					return _client.Activate(PrefetchedCurrent());
				}
			}
		}

		private object StreamLock()
		{
			return _client.Lock();
		}

		public virtual void Reset()
		{
			_remainingObjects = 0;
			_ids.Reset();
		}

		public virtual bool MoveNext()
		{
			lock (StreamLock())
			{
				if (_remainingObjects > 0)
				{
					--_remainingObjects;
					return SkipNulls();
				}
				Prefetch();
				--_remainingObjects;
				if (_remainingObjects < 0)
				{
					return false;
				}
				return SkipNulls();
			}
		}

		private bool SkipNulls()
		{
			// skip nulls (deleted objects)
			if (PrefetchedCurrent() == null)
			{
				return MoveNext();
			}
			return true;
		}

		private void Prefetch()
		{
			_client.Stream().WithEnvironment(new _IRunnable_67(this));
		}

		private sealed class _IRunnable_67 : IRunnable
		{
			public _IRunnable_67(ClientQueryResultIterator _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.EnsureObjectCacheAllocated(this._enclosing.PrefetchCount());
				this._enclosing._remainingObjects = Db4objects.Db4o.CS.Internal.ClientQueryResultIterator
					._prefetchingStrategy.PrefetchObjects(this._enclosing.Stream(), this._enclosing.
					_client.Transaction(), this._enclosing._ids, this._enclosing._prefetchedObjects, 
					this._enclosing.PrefetchCount());
				this._enclosing._prefetchRight = this._enclosing._remainingObjects;
			}

			private readonly ClientQueryResultIterator _enclosing;
		}

		private int PrefetchCount()
		{
			return Math.Max(Stream().Config().PrefetchObjectCount(), 1);
		}

		private ClientObjectContainer Stream()
		{
			return (ClientObjectContainer)_client.Stream();
		}

		private object PrefetchedCurrent()
		{
			return _prefetchedObjects[_prefetchRight - _remainingObjects - 1];
		}

		// TODO: open this as an external tuning interface in ExtObjectSet
		//	public void prefetch(int count){
		//		if(count < 1){
		//			count = 1;
		//		}
		//		i_prefetchCount = count;
		//		Object[] temp = new Object[i_prefetchCount];
		//		if(i_remainingObjects > 0){
		//			// Potential problem here: 
		//			// On reducing the prefetch size, this will crash.
		//			System.arraycopy(i_prefetched, 0, temp, 0, i_remainingObjects);
		//		}
		//		i_prefetched = temp;
		//	}
		private void EnsureObjectCacheAllocated(int prefetchObjectCount)
		{
			if (_prefetchedObjects == null)
			{
				_prefetchedObjects = new object[prefetchObjectCount];
				return;
			}
			if (prefetchObjectCount > _prefetchedObjects.Length)
			{
				object[] newPrefetchedObjects = new object[prefetchObjectCount];
				System.Array.Copy(_prefetchedObjects, 0, newPrefetchedObjects, 0, _prefetchedObjects
					.Length);
				_prefetchedObjects = newPrefetchedObjects;
			}
		}
	}
}
