/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class LazyClientObjectSetStub
	{
		private readonly AbstractQueryResult _queryResult;

		private IIntIterator4 _idIterator;

		public LazyClientObjectSetStub(AbstractQueryResult queryResult, IIntIterator4 idIterator
			)
		{
			_queryResult = queryResult;
			_idIterator = idIterator;
		}

		public virtual IIntIterator4 IdIterator()
		{
			return _idIterator;
		}

		public virtual AbstractQueryResult QueryResult()
		{
			return _queryResult;
		}

		public virtual void Reset()
		{
			_idIterator = _queryResult.IterateIDs();
		}
	}
}
