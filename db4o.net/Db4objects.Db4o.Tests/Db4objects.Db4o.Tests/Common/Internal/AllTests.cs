/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Internal.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(Db4objects.Db4o.Tests.Common.Internal.Convert.AllTests
				), typeof(Db4objects.Db4o.Tests.Common.Internal.Metadata.AllTests), typeof(Db4objects.Db4o.Tests.Common.Internal.Query.AllTests
				), typeof(BlockConverterTestCase), typeof(ClassMetadataTestCase), typeof(ClassMetadataTypeHandlerIntegrationTestCase
				), typeof(Comparable4TestCase), typeof(DeactivateTestCase), typeof(EmbeddedClientObjectContainerTestCase
				), typeof(EventDispatchersTestCase), typeof(InternalObjectContainerAPITestCase), 
				typeof(MarshallerFamilyTestCase), typeof(MarshallingBufferTestCase), typeof(MarshallingContextTestCase
				), typeof(Platform4TestCase), typeof(SerializerTestCase), typeof(TransactionLocalTestCase
				), typeof(TransactionTestCase) };
		}
	}
}
