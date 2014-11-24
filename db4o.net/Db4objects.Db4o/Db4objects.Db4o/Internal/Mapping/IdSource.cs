/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal.Mapping
{
	public class IdSource
	{
		private readonly IQueue4 _queue;

		public IdSource(IQueue4 queue)
		{
			_queue = queue;
		}

		public virtual bool HasMoreIds()
		{
			return _queue.HasNext();
		}

		public virtual int NextId()
		{
			return ((int)_queue.Next());
		}
	}
}
