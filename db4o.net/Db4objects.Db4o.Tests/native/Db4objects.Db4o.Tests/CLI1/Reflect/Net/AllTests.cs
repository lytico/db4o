/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI1.Reflect.Net
{
    using System;

	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new System.Type[]
				{
                    typeof(NetFieldTestCase),
					typeof(UninitializedObjectsWithFinalizerTestCase),
					typeof(UnavailableClassPrimitiveRepresentationTestCase),
				};
		}
	}
}
