/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Assorted.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(AliasesQueryingTestCase), typeof(AliasesTestCase
				), typeof(CallbackTestCase), typeof(CanUpdateFalseRefreshTestCase), typeof(CascadeDeleteDeletedTestCase
				), typeof(CascadedDeleteReadTestCase), typeof(ChangeIdentity), typeof(CommitTimeStampsNoSchemaChangesTestCase
				), typeof(CommitTimestampTestCase), typeof(CloseUnlocksFileTestCase), typeof(ComparatorSortTestCase
				), typeof(DatabaseGrowthSizeTestCase), typeof(DatabaseUnicityTest), typeof(DbPathDoesNotExistTestCase
				), typeof(DeleteSetTestCase), typeof(DeleteReaddChildReferenceTestSuite), typeof(
				DeleteUpdateTestCase), typeof(DescendToNullFieldTestCase), typeof(DualDeleteTestCase
				), typeof(ExceptionsOnNotStorableFalseTestCase), typeof(ExceptionsOnNotStorableIsDefaultTestCase
				), typeof(GetSingleSimpleArrayTestCase), typeof(HandlerRegistryTestCase), typeof(
				IndexCreateDropTestCase), typeof(IndexedBlockSizeQueryTestCase), typeof(InvalidOffsetInDeleteTestCase
				), typeof(KnownClassesTestCase), typeof(KnownClassesIndexTestCase), typeof(LazyObjectReferenceTestCase
				), typeof(LockedTreeTestCase), typeof(LongLinkedListTestCase), typeof(ManyRollbacksTestCase
				), typeof(MaximumDatabaseSizeTestCase), typeof(MultiDeleteTestCase), typeof(ObjectUpdateFileSizeTestCase
				), typeof(ObjectConstructorTestCase), typeof(ObjectContainerMemberTestCase), typeof(
				PlainObjectTestCase), typeof(PeekPersistedTestCase), typeof(PersistentIntegerArrayTestCase
				), typeof(PersistStaticFieldValuesTestSuite), typeof(PreventMultipleOpenTestCase
				), typeof(QueryByInterface), typeof(QueryingDoesNotProduceClassMetadataTestCase)
				, typeof(QueryingReadOnlyWithNewClassTestCase), typeof(ReAddCascadedDeleteTestCase
				), typeof(RenamingClassAfterQueryingTestCase), typeof(RepeatDeleteReaddTestCase)
				, typeof(RollbackDeleteTestCase), typeof(RollbackTestCase), typeof(RollbackUpdateTestCase
				), typeof(RollbackUpdateCascadeTestCase), typeof(SimplestPossibleNullMemberTestCase
				), typeof(SimplestPossibleTestCase), typeof(SimplestPossibleParentChildTestCase)
				, typeof(StaticFieldUpdateTestCase), typeof(StaticFieldUpdateConsistencyTestCase
				), typeof(SystemInfoTestCase), typeof(TransientCloneTestCase), typeof(UnknownReferenceDeactivationTestCase
				), typeof(WithTransactionTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(PersistTypeTestCase), typeof(ConcurrentRenameTestCase)
				 };
		}
		#endif // !SILVERLIGHT
	}
}
