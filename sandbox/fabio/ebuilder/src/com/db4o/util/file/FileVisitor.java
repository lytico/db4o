package com.db4o.util.file;

public interface FileVisitor {
	
	public final static int FILE = 1 << 0;
	public final static int DIRECTORY = 1 << 1;
	
	void visit(IFile child);

}
