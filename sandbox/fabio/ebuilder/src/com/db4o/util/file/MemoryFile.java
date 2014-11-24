package com.db4o.util.file;

import java.io.*;
import java.util.*;

import com.db4o.util.Bits;

public class MemoryFile implements IFile {

	private MemoryFile parent;
	private final String fileName;

	private byte[] content;
	private int length = 0;
	private Map<String, MemoryFile> children;
	private FileType type = FileType.None;
	private long lastModified = System.currentTimeMillis();

	enum FileType {
		None, File, Dir
	}

	public MemoryFile(MemoryFile parent, String file) {
		this.parent = parent;
		this.fileName = file;
	}

	public MemoryFile(String file) {
		this(null, file);
	}

	public MemoryFile() {
		this(null, "root");
	}

	@Override
	public String toString() {
		return "MemoryFile[" + getAbsolutePath() + "]";
	}

	public IFile file(String name) {

		int t = name.indexOf('/');

		if (t != -1) {
			String first = name.substring(0, t);
			return (t == 0 ? this : file(first)).file(name.substring(t + 1));
		}

		if ("..".equals(name)) {
			return parent();
		}

		MemoryFile file = children().get(name);

		if (file == null) {
			file = new MemoryFile(this, name);
			children().put(name, file);
		}

		return file;
	}

	public XMLParser xml() {
		return new XMLParserImpl(this);
	}

	public InputStream openInputStream() {
		return new ByteArrayInputStream(content, 0, length);
	}

	public String getAbsolutePath() {
		try {
			return realFile().getCanonicalPath();
		} catch (IOException e) {
			throw new RuntimeException(e);
		}
	}

	private File realFile() {
		throw new java.lang.UnsupportedOperationException();
	}

	public String name() {
		return fileName;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((fileName == null) ? 0 : fileName.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		MemoryFile other = (MemoryFile) obj;
		if (fileName == null) {
			if (other.fileName != null)
				return false;
		} else if (!fileName.equals(other.fileName))
			return false;
		return true;
	}

	private Map<String, MemoryFile> children() {
		if (children == null) {
			mkdir();
			children = new HashMap<String, MemoryFile>();
		}
		return children;
	}

	@Override
	public OutputStream openOutputStream(boolean append) {
		ByteArrayOutputStream buffer = new ByteArrayOutputStream() {
			@Override
			public void flush() throws IOException {
				super.flush();
				content = toByteArray();
				length = content.length;
				lastModified = System.currentTimeMillis();
			}
		};
		if (append && content != null) {
			buffer.write(content, 0, length);
		}
		mkfile();
		parent().mkdir();
		return buffer;
	}

	private void mkfile() {
		if (type == FileType.Dir) {
			throw new RuntimeException("Path is already a directory");
		}
		parent.mkdir();
		type = FileType.File;
	}

	@Override
	public void mkdir() {
		if (type == FileType.File) {
			throw new RuntimeException("Path is already a file");
		}
		type = FileType.Dir;
	}

	@Override
	public RandomAccessBuffer asBuffer() {
		if (content == null) {
			content = new byte[1024];
		}
		return new RandomAccessByteArray(content, length) {
			@Override
			public void flush() throws IOException {
				content = Arrays.copyOf(buffer(), length());
				length = length();
			}
		};
	}

	@Override
	public boolean exists() {
		return type != FileType.None;
	}

	@Override
	public MemoryFile parent() {
		return parent;
	}

	@Override
	public boolean exists(String fileName) {
		return children != null && children.containsKey(fileName);
	}

	@Override
	public void accept(FileVisitor visitor) {
		accept(visitor, 0xffffffff);
	}

	@Override
	public void accept(FileVisitor visitor, final int visitorOptions) {
		if (children == null)
			return;

		for (MemoryFile f : children.values()) {
			if (Bits.contains(visitorOptions, f.isFile() ? FileVisitor.FILE : FileVisitor.DIRECTORY)) {
				visitor.visit(f);
			}
		}
	}

	@Override
	public boolean isDirectory() {
		return type == FileType.Dir;
	}

	@Override
	public boolean isFile() {
		return type == FileType.File;
	}

	@Override
	public String getRelativePathTo(IFile base) {
		throw new java.lang.UnsupportedOperationException();
	}

	@Override
	public long lastModified() {
		return lastModified;
	}

	@Override
	public int copyTo(OutputStream out) throws IOException {
		out.write(content, 0, length);
		return length;
	}

	@Override
	public File nativeFile() {
		throw new java.lang.UnsupportedOperationException();
	}

}
