package com.db4o.util.file;

import java.io.*;

public class RandomAccessRealFile implements RandomAccessBuffer {
	
	private final File file;
	private RandomAccessFile delegate;

	public RandomAccessRealFile(File file) throws IOException {
		this.file = file;
		reset();
	}

	@Override
	public boolean isEmpty() {
		return file.exists() && file.length() == 0;
	}

	public int hashCode() {
		return delegate.hashCode();
	}

	public boolean equals(Object obj) {
		return delegate.equals(obj);
	}

	public String toString() {
		return delegate.toString();
	}

	public final void readFully(byte[] b) throws IOException {
		delegate.readFully(b);
	}

	public final void readFully(byte[] b, int off, int len) throws IOException {
		delegate.readFully(b, off, len);
	}

	public int skipBytes(int n) throws IOException {
		return delegate.skipBytes(n);
	}

	public void write(int b) throws IOException {
		delegate.write(b);
	}

	public void write(byte[] b) throws IOException {
		delegate.write(b);
	}

	public void write(byte[] b, int off, int len) throws IOException {
		delegate.write(b, off, len);
	}

	public void close() throws IOException {
		delegate.close();
	}

	public final boolean readBoolean() throws IOException {
		return delegate.readBoolean();
	}

	public final byte readByte() throws IOException {
		return delegate.readByte();
	}

	public final int readUnsignedByte() throws IOException {
		return delegate.readUnsignedByte();
	}

	public final short readShort() throws IOException {
		return delegate.readShort();
	}

	public final int readUnsignedShort() throws IOException {
		return delegate.readUnsignedShort();
	}

	public final char readChar() throws IOException {
		return delegate.readChar();
	}

	public final int readInt() throws IOException {
		return delegate.readInt();
	}

	public final long readLong() throws IOException {
		return delegate.readLong();
	}

	public final float readFloat() throws IOException {
		return delegate.readFloat();
	}

	public final double readDouble() throws IOException {
		return delegate.readDouble();
	}

	public final String readLine() throws IOException {
		return delegate.readLine();
	}

	public final String readUTF() throws IOException {
		return delegate.readUTF();
	}

	public final void writeBoolean(boolean v) throws IOException {
		delegate.writeBoolean(v);
	}

	public final void writeByte(int v) throws IOException {
		delegate.writeByte(v);
	}

	public final void writeShort(int v) throws IOException {
		delegate.writeShort(v);
	}

	public final void writeChar(int v) throws IOException {
		delegate.writeChar(v);
	}

	public final void writeInt(int v) throws IOException {
		delegate.writeInt(v);
	}

	public final void writeLong(long v) throws IOException {
		delegate.writeLong(v);
	}

	public final void writeFloat(float v) throws IOException {
		delegate.writeFloat(v);
	}

	public final void writeDouble(double v) throws IOException {
		delegate.writeDouble(v);
	}

	public final void writeBytes(String s) throws IOException {
		delegate.writeBytes(s);
	}

	public final void writeChars(String s) throws IOException {
		delegate.writeChars(s);
	}

	public final void writeUTF(String str) throws IOException {
		delegate.writeUTF(str);
	}

	@Override
	public void flush() throws IOException {
		delegate.getFD().sync();
	}

	@Override
	public int position() throws IOException {
		return (int) delegate.getFilePointer();
	}

	@Override
	public void position(int newPosition) throws IOException {
		delegate.seek(newPosition);
	}

	@Override
	public int length() throws IOException {
		return (int) delegate.length();
	}

	private void reset() throws FileNotFoundException {
		delegate = new RandomAccessFile(file, "rw");
	}

}
