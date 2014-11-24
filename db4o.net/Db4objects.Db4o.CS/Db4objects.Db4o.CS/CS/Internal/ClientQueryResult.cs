/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class ClientQueryResult : IdListQueryResult
	{
		public ClientQueryResult(Transaction ta) : base(ta)
		{
		}

		public ClientQueryResult(Transaction ta, int initialSize) : base(ta, initialSize)
		{
		}

		public override IEnumerator GetEnumerator()
		{
			return Skip(ClientServerPlatform.CreateClientQueryResultIterator(this));
		}
	}
}
