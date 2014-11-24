/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.io;

import com.db4o.io.*;

import db4ounit.extensions.*;
import db4ounit.fixtures.*;

/**
 * @sharpen.partial
 */
public class StorageTestSuite extends FixtureTestSuiteDescription implements OptOutVerySlow {

	/**
	 * @sharpen.ignore
	 */
	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
				new EnvironmentProvider(),
				new SubjectFixtureProvider(new Object[] {
						Db4oUnitPlatform.newPersistentStorage(),
						new MemoryStorage(),
						new PagingMemoryStorage(63),
						new CachingStorage(Db4oUnitPlatform.newPersistentStorage()),
				})			
		};
	}
	
	
	@Override
	public Class[] testUnits() {
		return new Class[] {
				BinTest.class,
				ReadOnlyBinTest.class,
				StorageTest.class				
		};
	}
		
//	combinationToRun(2);
}
