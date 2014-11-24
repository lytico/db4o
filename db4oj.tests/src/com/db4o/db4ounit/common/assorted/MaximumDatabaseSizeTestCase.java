/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.config.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class MaximumDatabaseSizeTestCase extends AbstractDb4oTestCase implements OptOutNetworkingCS, OptOutDefragSolo{
	
	public static void main(String[] args) {
		new MaximumDatabaseSizeTestCase().runAll();
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		configureMaximumFileSize(config);
	}

	private void configureMaximumFileSize(Configuration config) {
		Db4oLegacyConfigurationBridge.asFileConfiguration(config).maximumDatabaseFileSize(10000);
	}

	public void test() throws Exception{
		store(new SmallItem());
		boolean exceptionOccurred = false;
		boolean duringCommit = false;
		try {
			for (int i = 0; i < 100000; i++) {
				duringCommit = false;
				store(new BigItem());
				duringCommit = true;
				commit();
			}
		} catch (DatabaseMaximumSizeReachedException e) {
			exceptionOccurred = true;
		}
		Assert.isTrue(exceptionOccurred);
		fixture().configureAtRuntime(new RuntimeConfigureAction() {
			@Override
			public void apply(Configuration config) {
				configureMaximumFileSize(config);
				config.readOnly(true);
			}
		});
		reopen();
		SmallItem smallItem = retrieveOnlyInstance(SmallItem.class);
		Assert.isNotNull(smallItem);
		defragment();
		smallItem = retrieveOnlyInstance(SmallItem.class);
		Assert.isNotNull(smallItem);
	}
	
	public static class SmallItem {
		
	}
	
	public static class BigItem {
		public byte[] bytes = new byte[2];
	}

}
