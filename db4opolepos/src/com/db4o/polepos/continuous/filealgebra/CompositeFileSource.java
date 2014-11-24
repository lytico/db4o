/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.io.*;
import java.util.*;

public class CompositeFileSource implements FileSource {

	private List<FileSource> _sources;
	
	public CompositeFileSource(FileSource... sources) {
		this(Arrays.asList(sources));
	}

	public CompositeFileSource(List<FileSource> sources) {
		_sources = sources;
	}

	public List<File> files() {
		List<File> files = new ArrayList<File>();
		for (FileSource source : _sources) {
			files.addAll(source.files());
		}
		return files;
	}

}
