/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Linq.Internals
{
	public class ObjectSetWrapper<T> : ObjectSequence<T>
	{
		private IObjectSet _set;

		public int Count
		{
			get { return _set.Count; }
		}

		public ObjectSetWrapper(IObjectSet set) : base(set)
		{
			_set = set;
		}
	}
}
