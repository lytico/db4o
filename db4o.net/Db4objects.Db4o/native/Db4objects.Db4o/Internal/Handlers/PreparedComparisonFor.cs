/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal.Handlers
{
    internal class PreparedComparisonFor<T> : IPreparedComparison where T : IComparable<T>
    {
        private readonly T _source;

        public PreparedComparisonFor(T source)
        {
            _source = source;
        }

        public int CompareTo(object obj)
        {
			if (obj == null) return 1;
            return _source.CompareTo((T)obj);
        }
    }
}
