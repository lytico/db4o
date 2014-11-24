/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI1.Inside
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new System.Type[]
				{   
				    typeof(Query.QueryExpressionBuilderTestCase),
#if !SILVERLIGHT
					typeof(LegacyDb4oAssemblyNameRenamingTestCase),
#endif
				};
		}
	}
}
