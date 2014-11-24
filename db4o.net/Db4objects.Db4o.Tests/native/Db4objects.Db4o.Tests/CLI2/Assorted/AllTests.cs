/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override System.Type[] TestCases()
		{
			return new[]
			{
				typeof(AssemblyInfoTestCase),
				typeof(DelegateFieldTestCase),
#if !CF && !SILVERLIGHT
				typeof(DynamicallyLoadedAssemblyTestCase),
#endif
                typeof(ListOfNullableItemTestCase),
				typeof(NullableDateTimeTestCase),
                typeof(NullableTypes),
				typeof(NullableArraysElementsTestCase),
				typeof(SimpleGenericTypeTestCase),
				typeof(UntypedDelegateArrayTestCase),
			};
		}
	}
}
