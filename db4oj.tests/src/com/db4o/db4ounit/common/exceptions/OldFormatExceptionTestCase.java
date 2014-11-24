/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.util.*;
import com.db4o.ext.*;
import com.db4o.foundation.io.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.*;

/**
 * @exclude
 */
public class OldFormatExceptionTestCase implements TestCase, OptOutNoFileSystemData, OptOutWorkspaceIssue {

	public static void main(String[] args) {
		new ConsoleTestRunner(OldFormatExceptionTestCase.class).run();
	}
	
	// It is also regression test for COR-634.
	
	public void test() throws Exception {
		if (WorkspaceServices.workspaceRoot() == null) {
			System.err.println("Build environment not available. Skipping test case...");
			return;
		}
	    if (!File4.exists(sourceFile())) {
            System.err.println("Test source file " + sourceFile() + " not available. Skipping test case...");
            return;
        }

		
		final String oldDatabaseFilePath = oldDatabaseFilePath();
		
		Assert.expect(OldFormatException.class, new CodeBlock() {
			public void run() throws Throwable {
				Db4o.openFile(newConfiguration(false), oldDatabaseFilePath);
			}
		});
		
		ObjectContainer container = null;
		try {
			container = Db4o.openFile(newConfiguration(true), oldDatabaseFilePath);
		} finally {
			if (container != null) {
				container.close();
			}
		}
	}

	private Configuration newConfiguration(final boolean allowVersionUpdates) {
		final Configuration config = Db4o.newConfiguration();
		config.reflectWith(Platform4.reflectorForType(OldFormatExceptionTestCase.class));
		
		config.allowVersionUpdates(allowVersionUpdates);
		
		return config;
	}

	protected String oldDatabaseFilePath() throws IOException {
		final String oldFile = IOServices.buildTempPath("old_db.db4o");
		File4.copy(sourceFile(), oldFile);
		return oldFile;
	}
	
	private String sourceFile(){
        return WorkspaceServices.workspaceTestFilePath("db4oVersions/db4o_3.0.3");
    }
	
}
