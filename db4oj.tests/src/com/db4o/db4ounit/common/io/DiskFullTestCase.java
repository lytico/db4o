package com.db4o.db4ounit.common.io;

import java.io.*;

import db4ounit.*;


/**
 */
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class DiskFullTestCase extends DiskFullTestCaseBase {

	private static final long NO_SIZE_LIMIT = -1;
	
	public static void main(String[] arguments) {
		new ConsoleTestRunner(DiskFullTestCase.class).run();
	}

	public void testReleasesFileLocks() {
		assertReleasesFileLocks(false);
	}

	public void testReleasesFileLocksWithCache() {
		assertReleasesFileLocks(true);
	}

	public void testKeepsCommittedDataReadOnlyLimited() {
		assertKeepsCommittedDataReadOnlyLimited(false);
	}

	public void testKeepsCommittedDataReadOnlyLimitedWithCache() {
		assertKeepsCommittedDataReadOnlyLimited(true);
	}

	public void testKeepsCommittedDataReadWriteUnlimited() {
		assertKeepsCommittedDataReadWriteUnlimited(false);
	}

	public void testKeepsCommittedDataReadWriteUnlimitedWithCache() {
		assertKeepsCommittedDataReadWriteUnlimited(true);
	}

	private void assertReleasesFileLocks(boolean doCache) {
		openDatabase(NO_SIZE_LIMIT, false, doCache);
		triggerDiskFullAndClose();
		openDatabase(NO_SIZE_LIMIT, true, false);
		closeDb();
	}

	private void assertKeepsCommittedDataReadOnlyLimited(boolean doCache) {
		storeOneAndFail(NO_SIZE_LIMIT, doCache);
		assertItemsStored(1, curFileLength(), true, doCache);
	}

	private void assertKeepsCommittedDataReadWriteUnlimited(boolean doCache) {
		storeOneAndFail(NO_SIZE_LIMIT, doCache);
		assertItemsStored(1, NO_SIZE_LIMIT, false, doCache);
	}

	@Override
	protected void configureForFailure(ThrowCondition condition) {
		((LimitedSizeThrowCondition)condition).size(curFileLength());
	}

	@Override
	protected ThrowCondition createThrowCondition(Object conditionConfig) {
		return new LimitedSizeThrowCondition((Long) conditionConfig);
	}
	
	private long curFileLength() {
		return new File(tempFile()).length();
	}

}
