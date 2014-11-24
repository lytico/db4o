/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Internals
{
	internal class UnoptimizedQuery<T> : IDb4oLinqQueryInternal<T>
	{
		private IEnumerable<T> _result;

		public UnoptimizedQuery(IEnumerable<T> result)
		{
			if (result == null)
				throw new ArgumentNullException("result");

			_result = result;
		}

		public IEnumerable<T> Result
		{
			get { return _result; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _result.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region IDb4oLinqQueryInternal<T> Members

		public IEnumerable<T> UnoptimizedThenBy<TKey>(Func<T, TKey> function)
		{
			return ((IOrderedEnumerable<T>)_result).ThenBy(function);
		}

		public IEnumerable<T> UnoptimizedThenByDescending<TKey>(Func<T, TKey> function)
		{
			return ((IOrderedEnumerable<T>)_result).ThenByDescending(function);
		}

		public IEnumerable<T> UnoptimizedWhere(Func<T, bool> func)
		{
			return _result.Where(func);
		}

		#endregion
	}
}
