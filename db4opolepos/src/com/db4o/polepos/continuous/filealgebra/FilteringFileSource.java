/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.io.*;
import java.util.*;

public class FilteringFileSource implements FileSource {

	private FileSource _source;
	private FileFilter _filter;
	
	public FilteringFileSource(FileSource source, FileFilter filter) {
		_source = source;
		_filter = filter;
	}
	
	public List<File> files() {
		List<File> files = new ArrayList<File>();
		for (File file : _source.files()) {
			if(_filter.accept(file)) {
				files.add(file);
			}
		}
		return files;
	}

}
