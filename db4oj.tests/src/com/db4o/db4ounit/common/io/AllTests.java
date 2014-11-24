package com.db4o.db4ounit.common.io;

import db4ounit.*;
import db4ounit.extensions.*;


public class AllTests extends ComposibleTestSuite {
	
	public static void main(String[] arguments) {
		new ConsoleTestRunner(AllTests.class).run();
	}

	protected Class[] testCases() {
		return composeTests(new Class[] {
									BlockAwareBinTestSuite.class,
									DiskFullTestCase.class,
									MemoryBinGrowthTestCase.class,
									MemoryBinIsReusableTestCase.class,
									NonFlushingStorageTestCase.class,
									PagingMemoryStorageTestCase.class,
									RandomAccessFileStorageFactoryTestCase.class,
									// SaveAsStorageTestCase.class,  COR-2036
									StorageTestSuite.class,
									StackBasedDiskFullTestCase.class,
							});
	}
	

	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	protected Class[] composeWith() {
		return new Class[] { 
						BlockSizeDependentBinTestCase.class, 
						RandomAccessFileFactoryTestCase.class,
				};
	}
}