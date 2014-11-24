/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

#if !CF && !SILVERLIGHT
using Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL;
#endif

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			       	{
#if !CF && !SILVERLIGHT
						typeof(SslIntegrationTestCase),
			       		typeof(SslSocketTestCase), 
#endif
			       	};
		}
	}
}
