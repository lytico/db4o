/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Linq.Caching
{
	public interface ICache4<TKey, TValue>
	{
		TValue Produce(TKey key, Func<TKey, TValue> producer);
	}
}
