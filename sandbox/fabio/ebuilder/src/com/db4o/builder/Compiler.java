package com.db4o.builder;

import java.io.*;

import com.db4o.util.file.*;


public interface Compiler {

	enum Version {
		Java11("1.1"),
		Java12("1.2"),
		Java15("1.5"),
		Java16("1.6");
		
		private final String label;

		Version(String label) {
			this.label = label;
			
		}
		@Override
		public String toString() {
			return label;
		}
	}
	
	void sourceVersion(Version version);
	void targetVersion(Version version);

	void debugEnabled();

	void targetFolder(String path);

	void addClasspathEntry(IFile jar);

	void addSourceFile(IFile source);

	void outputWriter(PrintWriter out);
	void errorWriter(PrintWriter out);
	
	boolean compile();

}
