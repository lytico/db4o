/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.io.*;
import java.util.*;

public class FolderFileSource implements FileSource {

	private File _folder;
	
	public FolderFileSource(File folder) {
		_folder = folder;
	}
	
	public List<File> files() {
		return Arrays.asList(_folder.listFiles());
	}

}
