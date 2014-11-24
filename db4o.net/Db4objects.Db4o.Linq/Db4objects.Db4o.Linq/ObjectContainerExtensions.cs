/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq
{
	public static class ObjectContainerExtensions
	{
		/// <summary>
		/// This is the entry point of Linq to db4o.
		/// It allows the compiler to call the standard query operators
		/// in <see cref="Db4objects.Db4o.Linq.IDb4oLinqQuery">IDb4oLinqQuery</see>. The optimized methods are defined as extension methods
		/// on the <see cref="Db4objects.Db4o.Linq.IDb4oLinqQuery">IDb4oLinqQuery</see> marker interface.
		/// </summary>
		/// <typeparam name="T">The type to query the ObjectContainer</typeparam>
		/// <param name="self">A query factory (any IObjectContainer implementation)</param>
		/// <returns>A <see cref="Db4objects.Db4o.Linq.IDb4oLinqQuery">IDb4oLinqQuery</see> marker interface</returns>
		public static IDb4oLinqQuery<T> Cast<T>(this ISodaQueryFactory queryFactory)
		{
			if (typeof(T) == typeof(object)) return new PlaceHolderQuery<T>(queryFactory);
			return new Db4oQuery<T>(queryFactory);
		}

#if !CF_3_5
		public static IDb4oLinqQueryable<T> AsQueryable<T>(this ISodaQueryFactory queryFactory)
		{
			return queryFactory.Cast<T>().AsQueryable();
		}
#endif
	}
}
