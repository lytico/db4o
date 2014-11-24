/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.extensions.fixtures;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.extensions.util.*;

public abstract class AbstractFileBasedDb4oFixture extends AbstractSoloDb4oFixture {
	
	private final File _databaseFile;

	public AbstractFileBasedDb4oFixture() {
		final String fileName = fileName();
		_databaseFile = new File(CrossPlatformServices.databasePath(fileName));
	}

	protected abstract String fileName();
	
	protected ObjectContainer createDatabase(Configuration config) {
		return Db4o.openFile(config, getAbsolutePath());
	}

	public String getAbsolutePath() {
		return _databaseFile.getAbsolutePath();
	}

	public void defragment() throws Exception {
		defragment(getAbsolutePath());
	}

	protected void doClean() {
		if (_databaseFile.exists()) {
			_databaseFile.delete();
		}
	}

}
