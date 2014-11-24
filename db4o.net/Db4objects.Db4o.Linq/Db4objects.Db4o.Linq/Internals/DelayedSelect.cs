/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Internals
{
    class DelayedSelect<TOriginal, TProjection> : IDb4oLinqQueryInternal<TProjection>, IDelayedSelectOperation<TProjection>
    {
        private readonly IDelayedSelectOperation<TOriginal> query;
        private readonly Func<TOriginal, TProjection> projection;

        internal DelayedSelect(IDelayedSelectOperation<TOriginal> query, Func<TOriginal, TProjection> projection)
        {
            this.query = query;
            this.projection = projection;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TProjection> GetEnumerator()
        {
            return Enumerable.Select(query, projection).GetEnumerator();
        }

        public IEnumerable<TProjection> UnoptimizedThenBy<TKey>(Func<TProjection, TKey> function)
        {
            return Unoptimize().UnoptimizedThenBy(function);
        }

        public IEnumerable<TProjection> UnoptimizedThenByDescending<TKey>(Func<TProjection, TKey> function)
        {
            return Unoptimize().UnoptimizedThenByDescending(function);
        }

        public IEnumerable<TProjection> UnoptimizedWhere(Func<TProjection, bool> func)
        {
            return Unoptimize().UnoptimizedWhere(func);
        }

        public UnoptimizedQuery<TProjection> Unoptimize()
        {
            return new UnoptimizedQuery<TProjection>(Enumerable.Select(query, projection));
        }

        public IDb4oLinqQueryInternal<TProjection> Skip(int itemsToSkip)
        {
            return new UnoptimizedQuery<TProjection>(Enumerable.Select(query.Skip(itemsToSkip),projection));
        }
    }
}