/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.io.*;
import java.util.*;

public class SortedFileSource implements FileSource {

	private FileSource _source;
	private Comparator<File> _comparator;
	
	public SortedFileSource(FileSource source, Comparator<File> comparator) {
		_source = source;
		_comparator = comparator;
	}
	
	public List<File> files() {
		List<File> sorted = new ArrayList<File>(_source.files());
		Collections.sort(sorted, _comparator);
		return sorted;
	}

}
