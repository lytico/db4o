/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runAll();
    }

	protected Class[] testCases() {
		return composeTests(
				new Class[] {
						AliasesQueryingTestCase.class,
						AliasesTestCase.class,
			            CallbackTestCase.class,
			            CanUpdateFalseRefreshTestCase.class,
			            CascadeDeleteDeletedTestCase.class,
			            CascadedDeleteReadTestCase.class,
			            ChangeIdentity.class,
			            CommitTimeStampsNoSchemaChangesTestCase.class,
			            CommitTimestampTestCase.class,
			            CloseUnlocksFileTestCase.class,
			            ComparatorSortTestCase.class,
			            DatabaseGrowthSizeTestCase.class,
			            DatabaseUnicityTest.class,
			            DbPathDoesNotExistTestCase.class,
			            DeleteSetTestCase.class,
			            DeleteReaddChildReferenceTestSuite.class,
			            DeleteUpdateTestCase.class,
			            DescendToNullFieldTestCase.class,
			            DualDeleteTestCase.class,
			            ExceptionsOnNotStorableFalseTestCase.class,
			            ExceptionsOnNotStorableIsDefaultTestCase.class,
			            GetSingleSimpleArrayTestCase.class,
			            HandlerRegistryTestCase.class,
			            IndexCreateDropTestCase.class,
			            IndexedBlockSizeQueryTestCase.class,
			            InvalidOffsetInDeleteTestCase.class,
			            KnownClassesTestCase.class,
			            KnownClassesIndexTestCase.class,
			            LazyObjectReferenceTestCase.class,
			            LockedTreeTestCase.class,
			            LongLinkedListTestCase.class,
			            ManyRollbacksTestCase.class,
			            MaximumDatabaseSizeTestCase.class,
			            MultiDeleteTestCase.class,
			            ObjectUpdateFileSizeTestCase.class,
			            ObjectConstructorTestCase.class,
			            ObjectContainerMemberTestCase.class,
			            PlainObjectTestCase.class,
			            PeekPersistedTestCase.class,
			            PersistentIntegerArrayTestCase.class,
			            PersistStaticFieldValuesTestSuite.class,
			            PreventMultipleOpenTestCase.class,
			            QueryByInterface.class,
			            QueryingDoesNotProduceClassMetadataTestCase.class,
			            QueryingReadOnlyWithNewClassTestCase.class,
			            ReAddCascadedDeleteTestCase.class,
			            RenamingClassAfterQueryingTestCase.class,
			            RepeatDeleteReaddTestCase.class,
			            RollbackDeleteTestCase.class,
			            RollbackTestCase.class,
						RollbackUpdateTestCase.class,
						RollbackUpdateCascadeTestCase.class,
						SimplestPossibleNullMemberTestCase.class,
			            SimplestPossibleTestCase.class,
			            SimplestPossibleParentChildTestCase.class,
			            StaticFieldUpdateTestCase.class,
			            StaticFieldUpdateConsistencyTestCase.class,
			            SystemInfoTestCase.class,
			            TransientCloneTestCase.class,
			            UnavailableClassAsTreeSetElementTestCase.class,
			            UnknownReferenceDeactivationTestCase.class,
			            WithTransactionTestCase.class,
					});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] { 
				PersistTypeTestCase.class, 
				ConcurrentRenameTestCase.class, 
			};
	}
}
