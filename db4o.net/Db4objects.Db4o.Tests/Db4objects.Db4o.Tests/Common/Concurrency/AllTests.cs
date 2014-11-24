/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class AllTests : Db4oConcurrencyTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.AllTests().RunConcurrencyAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ArrayNOrderTestCase), typeof(ByteArrayTestCase), typeof(
				CascadeDeleteDeletedTestCase), typeof(CascadeDeleteFalseTestCase), typeof(CascadeOnActivateTestCase
				), typeof(CascadeOnUpdateTestCase), typeof(CascadeOnUpdate2TestCase), typeof(CascadeToVectorTestCase
				), typeof(CaseInsensitiveTestCase), typeof(Circular1TestCase), typeof(ClientDisconnectTestCase
				), typeof(CreateIndexInheritedTestCase), typeof(DeepSetTestCase), typeof(DeleteDeepTestCase
				), typeof(DifferentAccessPathsTestCase), typeof(ExtMethodsTestCase), typeof(GetAllTestCase
				), typeof(GreaterOrEqualTestCase), typeof(IndexedByIdentityTestCase), typeof(IndexedUpdatesWithNullTestCase
				), typeof(InternStringsTestCase), typeof(InvalidUUIDTestCase), typeof(IsStoredTestCase
				), typeof(MessagingTestCase), typeof(MultiDeleteTestCase), typeof(MultiLevelIndexTestCase
				), typeof(NestedArraysTestCase), typeof(ObjectSetIDsTestCase), typeof(ParameterizedEvaluationTestCase
				), typeof(PeekPersistedTestCase), typeof(PersistStaticFieldValuesTestCase), typeof(
				QueryForUnknownFieldTestCase), typeof(QueryNonExistantTestCase), typeof(ReadObjectNQTestCase
				), typeof(ReadObjectQBETestCase), typeof(ReadObjectSODATestCase), typeof(RefreshTestCase
				), typeof(UpdateObjectTestCase) };
		}
	}
}
#endif // !SILVERLIGHT
