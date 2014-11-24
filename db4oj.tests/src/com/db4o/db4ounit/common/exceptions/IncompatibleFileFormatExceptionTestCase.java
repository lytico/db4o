/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.io.*;

import db4ounit.*;

public class IncompatibleFileFormatExceptionTestCase implements TestLifeCycle {

	private final static String DB_PATH = "inmemory.db4o";
	private Storage storage;
	
	public static void main(String[] args) throws Exception {
		new ConsoleTestRunner(IncompatibleFileFormatExceptionTestCase.class).run();
	}

	public void setUp() throws Exception {
		storage = new MemoryStorage();
		Bin bin = storage.open(new BinConfiguration(DB_PATH, false, 0, false));
		bin.write(0, new byte[] { 1, 2, 3 }, 3);
		bin.close();
	}

	public void tearDown() throws Exception {
	}

	public void test() {
		final EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(storage);
		Assert.expect(IncompatibleFileFormatException.class, new CodeBlock() {
			public void run() throws Throwable {
				Db4oEmbedded.openFile(config, DB_PATH);
			}
		});
	}


}
