/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.api;

import com.db4o.foundation.io.*;

import db4ounit.*;

/**
 * @sharpen.partial
 */
public class TestWithTempFile implements TestLifeCycle {

	private String _tempFile;
	
	/**
	 * @sharpen.ignore
	 */
	protected String tempFile() {
		if (null == _tempFile)
			 _tempFile = Path4.getTempFileName();
		
		return _tempFile;
	}

	public void setUp() throws Exception {
	}

	public void tearDown() throws Exception {
		File4.delete(tempFile());
	}

}