/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal
{
	/// <summary>Platform specific defaults.</summary>
	/// <remarks>Platform specific defaults.</remarks>
	public class ClientServerPlatform
	{
		/// <summary>
		/// The default
		/// <see cref="ClientQueryResultIterator">ClientQueryResultIterator</see>
		/// for this platform.
		/// </summary>
		/// <returns></returns>
		public static IEnumerator CreateClientQueryResultIterator(AbstractQueryResult result
			)
		{
			IQueryResultIteratorFactory factory = result.Config().QueryResultIteratorFactory(
				);
			if (null != factory)
			{
				return factory.NewInstance(result);
			}
			return new ClientQueryResultIterator(result);
		}
	}
}
