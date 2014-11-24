/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI2.Regression
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override System.Type[] TestCases()
		{
#if !MONO
			return new System.Type[] {
				typeof(COR242TestCase),
				typeof(COR195TestCase),
			};
#else
			return new System.Type[0];
#endif
		}
	}
}
