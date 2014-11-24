/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Internals
{
	internal class PlaceHolderQuery<T> : IDb4oLinqQuery<T>
	{
		private ISodaQueryFactory _queryFactory;

		public ISodaQueryFactory QueryFactory
		{
			get { return _queryFactory; }
		}

		public PlaceHolderQuery(ISodaQueryFactory queryFactory)
		{
			_queryFactory = queryFactory;
		}

		public IEnumerator<T> GetEnumerator()
		{
			var query = _queryFactory.Query();
			query.Constrain(typeof(T));
			return new ObjectSequence<T>(query.Execute()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
