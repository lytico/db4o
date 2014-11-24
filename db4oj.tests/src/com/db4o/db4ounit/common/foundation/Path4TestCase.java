/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.io.*;

import db4ounit.*;

/**
 * @exclude
 */
public class Path4TestCase implements TestCase{
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	public void testGetTempFileName(){
		String tempFileName = Path4.getTempFileName();
		Assert.isTrue(File4.exists(tempFileName));
		File4.delete(tempFileName);
	}
	
}
