/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.Internal
{
	public interface IQueryResultIteratorFactory
	{
		IEnumerator NewInstance(AbstractQueryResult result);
	}
}
