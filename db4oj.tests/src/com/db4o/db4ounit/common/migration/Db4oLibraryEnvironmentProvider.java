/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.migration;

import java.io.*;
import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Db4oLibraryEnvironmentProvider {
	
	private final Map<String, Db4oLibraryEnvironment> _environments = new HashMap();
	private final File _classPath;
	
	public Db4oLibraryEnvironmentProvider(File classPath) {
		_classPath = classPath;
	}
	
	public Db4oLibraryEnvironment environmentFor(final String path)
			throws IOException {
		Db4oLibraryEnvironment existing = existingEnvironment(path);
		if (existing != null) return existing;
		return newEnvironment(path);
	}

	private Db4oLibraryEnvironment existingEnvironment(String path) {
		return _environments.get(path);
	}

	private Db4oLibraryEnvironment newEnvironment(String path)
			throws IOException {
		Db4oLibraryEnvironment env = new Db4oLibraryEnvironment(new File(path), _classPath);
		_environments.put(path, env);
		return env;
	}

	public void disposeAll() {
		for (Db4oLibraryEnvironment e : _environments.values()) {
			e.dispose();
		}
		_environments.clear();
    }

}