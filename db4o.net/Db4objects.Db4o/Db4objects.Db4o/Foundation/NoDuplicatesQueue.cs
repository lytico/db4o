/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class NoDuplicatesQueue : IQueue4
	{
		private IQueue4 _queue;

		private Hashtable4 _seen;

		public NoDuplicatesQueue(IQueue4 queue)
		{
			_queue = queue;
			_seen = new Hashtable4();
		}

		public virtual void Add(object obj)
		{
			if (_seen.ContainsKey(obj))
			{
				return;
			}
			_queue.Add(obj);
			_seen.Put(obj, obj);
		}

		public virtual bool HasNext()
		{
			return _queue.HasNext();
		}

		public virtual IEnumerator Iterator()
		{
			return _queue.Iterator();
		}

		public virtual object Next()
		{
			return _queue.Next();
		}

		public virtual object NextMatching(IPredicate4 condition)
		{
			return _queue.NextMatching(condition);
		}
	}
}
