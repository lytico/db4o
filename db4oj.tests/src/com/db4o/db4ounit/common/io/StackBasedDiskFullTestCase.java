package com.db4o.db4ounit.common.io;



/**
 */
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class StackBasedDiskFullTestCase extends DiskFullTestCaseBase {

	public void testFailDuringCommitParticipants() {
		assertFailDuringCommit(1, false);
	}

	public void testFailDuringCommitParticipantsWithCache() {
		assertFailDuringCommit(1, true);
	}

	public void testFailDuringCommitWriteChanges() {
		assertFailDuringCommit(1, false);
	}

	public void testFailDuringCommitWriteChangesWithCache() {
		assertFailDuringCommit(1, true);
	}

	private void assertFailDuringCommit(int hitThreshold, boolean doCache) {
		StackBasedConfiguration config = new StackBasedConfiguration(
				"com.db4o.internal.LocalTransaction",
				"commitParticipants",
				hitThreshold);
		storeNAndFail(config, 95, 10, doCache);
		assertItemsStored(90, config, false, doCache);
	}
	
	protected void configureForFailure(ThrowCondition condition) {
		((StackBasedLimitedSpaceThrowCondition)condition).enabled(true);
	}

	protected ThrowCondition createThrowCondition(Object conditionConfig) {
		return new StackBasedLimitedSpaceThrowCondition((StackBasedConfiguration)conditionConfig);
	}

}
