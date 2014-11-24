/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.filelock;

import com.db4o.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.db4ounit.util.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.IOServices.ProcessRunner;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class DatabaseFileLockedAcrossVMTestCase
	extends TestWithTempFile
	implements OptOutInMemory, OptOutNoInheritedClassPath, OptOutWorkspaceIssue {
	
	
	public void testLockedFile() throws Exception {
		ProcessRunner externalVM = JavaServices.startJava(AcquireNativeLock.class.getName(), new String[]{ tempFile() });		
		
		waitToFinish(externalVM);
		
		try {
			Assert.expect(DatabaseFileLockedException.class, new CodeBlock() {
				public void run() throws Throwable {
					Db4oEmbedded.openFile(Db4oEmbedded.newConfiguration(), tempFile());
				}
			});
		} finally {
			externalVM.write("");
			try {
				externalVM.waitFor();
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}


	private void waitToFinish(ProcessRunner process) {
		try {
			process.waitFor("ready", 3000);
		} catch (Exception ex) {
			process.destroy();
			throw new RuntimeException(ex);
		}			
	}	

	public static void main(String[] args) {
		new ConsoleTestRunner(DatabaseFileLockedAcrossVMTestCase.class).run();
	}
}
