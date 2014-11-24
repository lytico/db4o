/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

import java.io.*;
import java.util.*;
import java.util.regex.*;

import com.db4o.polepos.continuous.filealgebra.*;

public class Db4oJarSortedFileSource implements FileSource {

	private final static Pattern JAR_NAME_PATTERN = Pattern.compile("db4o.*(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)-.*\\.jar");
	private final static int REVISION_GROUP_IDX = 4;

	private FileSource _source;
	
	public Db4oJarSortedFileSource(FileSource source) {
		_source = new SortedFileSource(new FilteringFileSource(source, new Db4oJarFileFilter()), new Db4oJarComparator());
	}

	public List<File> files() {
		return _source.files();
	}

	private static final class Db4oJarFileFilter implements FileFilter {
		public boolean accept(File file) {
			return JAR_NAME_PATTERN.matcher(file.getName()).matches();
		}
	}
	
	private static final class Db4oJarComparator implements Comparator<File> {
		
		public int compare(File file1, File file2) {
			return ((Integer)revision(file2)).compareTo(revision(file1));
		}

		private int revision(File file) {
			Matcher matcher = JAR_NAME_PATTERN.matcher(file.getName());
			if(!matcher.matches()) {
				throw new IllegalArgumentException("Not a db4o jar: " + file.getAbsolutePath());
			}
			return Integer.parseInt(matcher.group(REVISION_GROUP_IDX));
		}
	}

}
