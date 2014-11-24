/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class IdListQueryResultTestCase : QueryResultTestCase
	{
		public static void Main(string[] args)
		{
			new IdListQueryResultTestCase().RunSolo();
		}

		protected override AbstractQueryResult NewQueryResult()
		{
			return new IdListQueryResult(Trans());
		}
	}
}
