/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.util;

/**
 * @exclude
 */
public class WorkspaceLocations {
	private static String _testFolder = null;

	public static String getTestFolder() {
		if (_testFolder == null) {
			_testFolder = WorkspaceServices.workspacePath("db4oj.tests/test");
		}
		return _testFolder;
	}
}
