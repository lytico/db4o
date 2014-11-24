/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.CS.Caching;

namespace Db4objects.Db4o.Tests.Common.CS.Caching
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ClientSlotCacheTestCase) };
		}
	}
}
#endif // !SILVERLIGHT
