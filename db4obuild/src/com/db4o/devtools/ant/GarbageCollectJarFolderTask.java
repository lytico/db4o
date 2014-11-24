/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.devtools.ant;

import java.io.*;
import java.util.*;

import org.apache.tools.ant.*;

public class GarbageCollectJarFolderTask extends Task {

	private File _jarFolder;
	private int _maxFileCount;
	
	public void setJarFolder(File jarFolder) {
		_jarFolder = jarFolder;
	}
	
	public void setMaxFiles(int maxFileCount) {
		_maxFileCount = maxFileCount;
	}
	
	public void execute() throws BuildException {
		if(!_jarFolder.isDirectory()) {
			throw new BuildException("jarFolder - not an existing directory: " + _jarFolder);
		}
		File[] files = _jarFolder.listFiles();
		if(files.length >= _maxFileCount) {
			Arrays.sort(files, new Comparator<File>() {
				public int compare(File f1, File f2) {
					return new Long(f1.lastModified()).compareTo(f2.lastModified());
				}
			});
			for(int fileIdx = 0; fileIdx < files.length - _maxFileCount; fileIdx++) {
				System.err.println("DELETE " + files[fileIdx]);
				files[fileIdx].delete();
			}
		}
	}
	
}
