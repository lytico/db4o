/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.defragment;

import com.db4o.db4ounit.common.api.*;

public class DefragmentTestCaseBase extends Db4oTestWithTempFile {
	
	protected String sourceFile() {
		return tempFile();
	}
	
	protected String backupFile() {
		return backupFileNameFor(tempFile());
	}

	public static String backupFileNameFor(String file) {
		return file + ".backup";
	}
}
