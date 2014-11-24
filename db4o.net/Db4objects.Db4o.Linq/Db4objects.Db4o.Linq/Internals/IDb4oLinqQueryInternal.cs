namespace Db4objects.Db4o.Linq.Internals
{
	using System;
	using System.Collections.Generic;

	interface IDb4oLinqQueryInternal<T> : IDb4oLinqQuery<T>
	{
		IEnumerable<T> UnoptimizedThenBy<TKey>(Func<T, TKey> function);

		IEnumerable<T> UnoptimizedThenByDescending<TKey>(Func<T, TKey> function);

		IEnumerable<T> UnoptimizedWhere(Func<T, bool> func);
	}
}
