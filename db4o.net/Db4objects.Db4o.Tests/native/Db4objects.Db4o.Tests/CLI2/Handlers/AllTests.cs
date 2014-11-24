/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
    public class AllTests : Db4oTestSuite
    {
        protected override Type[] TestCases()
        {
            return new Type[]
                   	{
						typeof(CustomGenericCollectionTestCase),
						typeof(ClassDerivedFromGenericCollectionsTestSuite),
                   		typeof(TypeHandlerConfigurationTestCase),
#if !SILVERLIGHT //TODO: Investigate failure on Silverlight
						typeof(GenericCollectionTypeHandlerTestSuite),
						typeof(GenericCollectionTypeHandlerGreaterSmallerTestSuite)
#endif
                   	};
        }
    }
}
