/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.polepos.continuous;

import java.io.*;
import java.util.*;

public class Db4oJarCollection {
	private final File _currentJar;
	private final Set<File> _otherJars;
	
	public Db4oJarCollection(File currentJar, List<File> otherJars) {
		_currentJar = currentJar;
		_otherJars = Collections.unmodifiableSet(new HashSet<File>(otherJars));
	}
	
	public Set<File> otherJars() {
		return _otherJars;
	}
	
	public File currentJar() {
		return _currentJar;
	}
}
