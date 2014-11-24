/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.SharpenLang
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new[]
				{
#if !CF && !SILVERLIGHT
					typeof(DynamicallyLoadedAssemblyTestCase),
					typeof(SharpenRuntimeTestCase),
#endif
                    typeof(TypeReferenceTestCase),
				};
		}
	}
}
