/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Db4objects.Db4o.Linq.Expressions;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq
{
	/// <summary>
	/// A class that defines some standard linq query operations
	/// that can be optimized.
	/// </summary>
	public static class Db4oLinqQueryExtensions
	{
		public static IDb4oLinqQuery<TSource> Where<TSource>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, bool>> expression)
		{
			return Process(self,
				query => new WhereClauseVisitor().Process(expression),
				data => data.UnoptimizedWhere(expression.Compile())
			);
		}

        public static IDb4oLinqQuery<TSource> Skip<TSource>(this IDb4oLinqQuery<TSource> query, int itemsToSkip)
        {
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

        	var skipable = query as IDelayedSelectOperation<TSource>;
            if (null != skipable)
            {
                return new UnoptimizedQuery<TSource>(skipable.Skip(itemsToSkip));
            }

        	return new UnoptimizedQuery<TSource>(Enumerable.Skip(query, itemsToSkip));
        } 

		public static int Count<TSource>(this IDb4oLinqQuery<TSource> self)
		{
			if (self == null)
				throw new ArgumentNullException("self");

			var query = self as Db4oQuery<TSource>;
			if (query != null)
				return query.Count;

			return Enumerable.Count(self);
		}

		private static IDb4oLinqQuery<TSource> Process<TSource>(
			IDb4oLinqQuery<TSource> query,
			Func<Db4oQuery<TSource>, IQueryBuilderRecord> queryProcessor,
			Func<IDb4oLinqQueryInternal<TSource>, IEnumerable<TSource>> fallbackProcessor)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

			var candidate = query as Db4oQuery<TSource>;

			if (candidate == null)
			{
				return new UnoptimizedQuery<TSource>(fallbackProcessor((IDb4oLinqQueryInternal<TSource>) EnsureDb4oQuery(query)));
			}
			try
			{
				IQueryBuilderRecord record = queryProcessor(candidate);
				return new Db4oQuery<TSource>(candidate, record);
			}
			catch (QueryOptimizationException)
			{
				return new UnoptimizedQuery<TSource>(fallbackProcessor(candidate));
			}
		}

		private static IDb4oLinqQuery<TSource> EnsureDb4oQuery<TSource>(IDb4oLinqQuery<TSource> query)
		{
			var placeHolderQuery = query as PlaceHolderQuery<TSource>;
			if (placeHolderQuery == null)
			{
				return query;
			}

			return new Db4oQuery<TSource>(placeHolderQuery.QueryFactory);
		}

		private static IDb4oLinqQuery<TSource> ProcessOrderBy<TSource, TKey>(
			IDb4oLinqQuery<TSource> query,
			OrderByClauseVisitorBase visitor,
			Expression<Func<TSource, TKey>> expression,
			Func<IDb4oLinqQueryInternal<TSource>, IEnumerable<TSource>> fallbackProcessor)
		{
			return Process(query, q => visitor.Process(expression), fallbackProcessor);
		}

		public static IDb4oLinqQuery<TSource> OrderBy<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByAscendingClauseVisitor(), expression,
				data => data.OrderBy(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TSource> OrderByDescending<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByDescendingClauseVisitor(), expression,
				data => data.OrderByDescending(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TSource> ThenBy<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByAscendingClauseVisitor(), expression,
				data => data.UnoptimizedThenBy(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TSource> ThenByDescending<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByDescendingClauseVisitor(), expression,
				data => data.UnoptimizedThenByDescending(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TRet> Select<TSource, TRet>(this IDb4oLinqQuery<TSource> self,
            Func<TSource, TRet> selector)
		{
			var placeHolderQuery = self as PlaceHolderQuery<TSource>;
			if (placeHolderQuery != null) return new Db4oQuery<TRet>(placeHolderQuery.QueryFactory);

            var asDb4oQuery = self as IDelayedSelectOperation<TSource>;
            if (asDb4oQuery != null) return new DelayedSelect<TSource, TRet>(asDb4oQuery, selector);

			return new UnoptimizedQuery<TRet>(Enumerable.Select(self, selector));
		}
#if !CF_3_5
		public static IDb4oLinqQueryable<TSource> AsQueryable<TSource>(this IDb4oLinqQuery<TSource> self)
		{
			return new Db4oQueryable<TSource>(self);
		}
#endif
	}
}