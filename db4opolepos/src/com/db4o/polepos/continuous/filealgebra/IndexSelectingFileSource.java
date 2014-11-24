/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.io.*;
import java.util.*;

public class IndexSelectingFileSource implements FileSource {

	private FileSource _source;
	private int[] _indices;
	
	public IndexSelectingFileSource(FileSource source, int... indices) {
		_source = source;
		_indices = indices;
		if(_indices.length < 1) {
			throw new IllegalArgumentException();
		}
		for (int selIdx : _indices) {
			if(selIdx < 0) {
				throw new IllegalArgumentException();
			}
		}
		Arrays.sort(_indices);
	}
	
	public List<File> files() {
		List<File> allFiles = _source.files();
		int[] indices = indices(allFiles.size());
		List<File> selectedFiles = new ArrayList<File>(indices.length);
		for (Integer index : indices) {
			if(index >= allFiles.size()) {
				throw new IllegalArgumentException();
			}
			selectedFiles.add(allFiles.get(index));
		}
		return selectedFiles;
	}
	
	protected int[] indices(int count) {
		return _indices;
	}

}
