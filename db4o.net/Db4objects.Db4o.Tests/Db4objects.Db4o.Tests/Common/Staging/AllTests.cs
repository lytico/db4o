/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Staging.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ActivateDepthTestCase), typeof(InterfaceQueryTestCase
				), typeof(GenericClassWithExistingSuperClassTestCase), typeof(LazyQueryDeleteTestCase
				), typeof(OldVersionReflectFieldAfterRefactorTestCase), typeof(StoredClassUnknownClassQueryTestCase
				), typeof(UntypedFieldSortingTestCase) });
		}

		// COR-1131
		// COR-1959
		// COR-1937
		// COR-1542
		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(ClientServerPingTestCase), typeof(DeepPrefetchingCacheConcurrencyTestCase
				), typeof(OwnCommitCallbackFlaggedEmbeddedTestSuite), typeof(PingTestCase), typeof(
				TAUnavailableClassAtServer) };
		}
		#endif // !SILVERLIGHT
		// COR-1762
		// COR-1964
		//COR-1987
	}
}
