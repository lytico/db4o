package com.db4o.db4ounit.common.handlers;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.convert.*;

import db4ounit.*;

public class CommitTimestampMigrationTestCase extends FormatMigrationTestCaseBase {
	
	
	public static class Item {
	}

	@Override
	protected void configureForTest(Configuration config) {
		configureForStore(config);
	}
	
	@Override
	protected void configureForStore(Configuration config) {
		config.generateVersionNumbers(ConfigScope.GLOBALLY);
			
		// This needs to be in a different method for .NET because .NET
		// tries to resolve the complete method body for jitting and will
		// throw without calling the first method. 

		configureForStore8_0AndNewer(config);
	}
	
    protected void configureForStore8_0AndNewer(Configuration config){
    	config.generateCommitTimestamps(true);
    }

	
	@Override
	protected void assertObjectsAreReadable(ExtObjectContainer objectContainer) {
		
		if (db4oMajorVersion() <= 6 || (db4oMajorVersion() == 7 && db4oMinorVersion() == 0)) {
			return;
		}			
		
		Item item = objectContainer.query(Item.class).next();
		ObjectInfo objectInfo = objectContainer.getObjectInfo(item);
		long version = objectInfo.getCommitTimestamp();
		Assert.isGreater(0, version);
	}

	@Override
	protected String fileNamePrefix() {
		return "commitTimestamp";
	}

	@Override
	protected void store(ObjectContainerAdapter objectContainer) {
		objectContainer.store(new Item());
	}

}
