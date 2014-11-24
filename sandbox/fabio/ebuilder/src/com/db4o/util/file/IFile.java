package com.db4o.util.file;

import java.io.*;

public interface IFile {

	IFile file(String name);

	XMLParser xml();

	InputStream openInputStream();
	OutputStream openOutputStream(boolean append);

	String getAbsolutePath();

	String name();

	RandomAccessBuffer asBuffer();

	boolean exists();
	boolean exists(String fileName);

	IFile parent();
	
	void mkdir();

	void accept(FileVisitor visitor);
	void accept(FileVisitor visitor, int visitorOptions);

	boolean isDirectory();

	boolean isFile();

	String getRelativePathTo(IFile base);

	long lastModified();

	int copyTo(OutputStream out) throws IOException;

	File nativeFile();


}
