/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class DelegatingBlockingQueue : IBlockingQueue4
	{
		private IBlockingQueue4 queue;

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		public virtual object Next(long timeout)
		{
			return queue.Next(timeout);
		}

		public virtual object Next()
		{
			return queue.Next();
		}

		public virtual void Add(object obj)
		{
			queue.Add(obj);
		}

		public virtual bool HasNext()
		{
			return queue.HasNext();
		}

		public virtual object NextMatching(IPredicate4 condition)
		{
			return queue.NextMatching(condition);
		}

		public virtual IEnumerator Iterator()
		{
			return queue.Iterator();
		}

		public DelegatingBlockingQueue(IBlockingQueue4 queue)
		{
			this.queue = queue;
		}

		public virtual void Stop()
		{
			queue.Stop();
		}

		public virtual int DrainTo(Collection4 list)
		{
			return queue.DrainTo(list);
		}
	}
}
