/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI2.Collections
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override System.Type[] TestCases()
		{
			return new System.Type[]
			{ 
                typeof(ArrayDictionary4TestCase),
                typeof(ArrayDictionary4TATestCase),
				typeof(ArrayDictionary4TransparentPersistenceTestCase),

                typeof(ArrayList4TATestCase), 
#if !SILVERLIGHT
				typeof(ArrayList4ActivatableTestCase), 
				typeof(ArrayList4TestCase), 
                typeof(BindingListTestCase),
#endif
                typeof(GenericDictionaryTestCase),
				typeof(GenericDictionaryTestSuite),

#if NET_3_5 && ! CF
                typeof(HashSetTestCase),
#endif 
				typeof(Transparent.AllTests),
            };
		}
	}
}
