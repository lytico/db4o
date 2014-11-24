package com.db4o.util.file;

import java.io.*;

import com.db4o.util.Bits;

public class RealFile implements IFile {

	private RealFile parent;
	private final File realFile;

	public RealFile(RealFile parent, File file) {
		this.parent = parent;
		this.realFile = file;
	}

	public RealFile(String name) {
		this(new File(name));
	}

	public RealFile(File file) {
		this(null, file);
	}

	@Override
	public String toString() {
		return "RealFile[" + getAbsolutePath() + "]";
	}

	public IFile file(String name) {

		int t = name.indexOf('/');
		
		if (t == 0) {
			return root().file(name.substring(1));
		}

		if (t != -1) {
			String first = name.substring(0, t);
			return (t == 0 ? this : file(first)).file(name.substring(t + 1));
		}

		if ("..".equals(name)) {
			return parent();
		}

		return file(new File(realFile(), name));
	}

	private RealFile root() {
		return new RealFile(new File("/"));
	}

	private RealFile file(File file) {
		return new RealFile(this, file);
	}

	public XMLParser xml() {
		return new XMLParserImpl(this);
	}

	private File realFile() {
		return realFile;
	}

	public InputStream openInputStream() {
		mkParents();
		try {
			return new FileInputStream(realFile());
		} catch (FileNotFoundException e) {
			throw new RuntimeException(e);
		}
	}

	private void mkParents() {
		if (parent != null) {
			parent.mkdir();
		}
	}

	@Override
	public void mkdir() {
		if (parent != null) {
			parent.mkdir();
		}
		if (!realFile().exists()) {
			realFile().mkdirs();
		}
	}

	public String getAbsolutePath() {
		try {
			return realFile().getCanonicalPath();
		} catch (IOException e) {
			throw new RuntimeException(e);
		}
	}

	public String name() {
		return realFile().getName();
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((realFile == null) ? 0 : realFile.hashCode());
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
		RealFile other = (RealFile) obj;
		if (realFile == null) {
			if (other.realFile != null)
				return false;
		} else if (!realFile.equals(other.realFile))
			return false;
		return true;
	}

	@Override
	public OutputStream openOutputStream(boolean append) {
		mkParents();

		try {
			return new FileOutputStream(realFile(), append);
		} catch (FileNotFoundException e) {
			throw new RuntimeException(e);
		}
	}

	@Override
	public RandomAccessBuffer asBuffer() {
		mkParents();

		try {
			return new RandomAccessRealFile(realFile());
		} catch (IOException e) {
			throw new RuntimeException(e);
		}
	}

	@Override
	public boolean exists() {
		return realFile().exists();
	}

	@Override
	public IFile parent() {
		if (parent == null) {
			File parentFile = realFile().getAbsoluteFile().getParentFile();
			if (parentFile != null) {
				parent = new RealFile(parentFile);
			}
		}
		return parent;
	}

	@Override
	public boolean exists(String fileName) {
		return new File(realFile(), fileName).exists();
	}

	@Override
	public void accept(final FileVisitor visitor) {
		accept(visitor, 0xffffffff);
	}

	@Override
	public void accept(final FileVisitor visitor, final int visitorOptions) {
		realFile().listFiles(new FileFilter() {
			@Override
			public boolean accept(File file) {
				if (Bits.contains(visitorOptions, file.isFile() ? FileVisitor.FILE : FileVisitor.DIRECTORY)) {
					visitor.visit(file(file));
				}
				return false;
			}
		});
	}

	@Override
	public boolean isDirectory() {
		return realFile().isDirectory();
	}

	@Override
	public boolean isFile() {
		return realFile().isFile();
	}

	@Override
	public String getRelativePathTo(IFile base) {
		String baseAbsolutePath = base.getAbsolutePath();
		String absolutePath = getAbsolutePath();
		if (baseAbsolutePath.length() == absolutePath.length()) {
			return "";
		}
		return absolutePath.substring(baseAbsolutePath.length()+1);
	}

	@Override
	public long lastModified() {
		return realFile().lastModified();
	}

	@Override
	public int copyTo(OutputStream out) throws IOException {
		byte[] buffer = new byte[1024 * 4];
		InputStream in = openInputStream();
		int read;
		int total = 0;
		while((read=in.read(buffer))!= -1) {
			total += read;
			out.write(buffer, 0, read);
		}
		in.close();
		return total;
	}

	@Override
	public File nativeFile() {
		return realFile();
	}

}
