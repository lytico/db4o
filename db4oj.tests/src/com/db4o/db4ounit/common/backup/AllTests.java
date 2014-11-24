package com.db4o.db4ounit.common.backup;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return composeTests(
				new Class[] {
						BackupFromMemoryBinIsAccessibleThroughStorageTestCase.class,
						BackupMemoryToFileTestCase.class,
				});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
						BackupStressTestCase.class,
						BackupSyncStressTestCase.class
				};
	}
}
