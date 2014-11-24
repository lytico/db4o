package com.db4o.util.file;

import java.io.*;

public class DelegatingRandomAccessBuffer implements RandomAccessBuffer {

	private RandomAccessBuffer delegate;

	public DelegatingRandomAccessBuffer(RandomAccessBuffer delegate) {
		this.delegate = delegate;
	}

	@Override
	public void close() throws IOException {
		delegate.close();
	}

	@Override
	public void flush() throws IOException {
		delegate.flush();
	}

	@Override
	public int position() throws IOException {
		return delegate.position();
	}

	@Override
	public void position(int newPosition) throws IOException {
		delegate.position(newPosition);
	}

	@Override
	public int length() throws IOException {
		return delegate.length();
	}

	@Override
	public boolean isEmpty() {
		return delegate.isEmpty();
	}

	@Override
	public boolean readBoolean() throws IOException {
		return delegate.readBoolean();
	}

	@Override
	public byte readByte() throws IOException {
		return delegate.readByte();
	}

	@Override
	public char readChar() throws IOException {
		return delegate.readChar();
	}

	@Override
	public double readDouble() throws IOException {
		return delegate.readDouble();
	}

	@Override
	public float readFloat() throws IOException {
		return delegate.readFloat();
	}

	@Override
	public void readFully(byte[] b, int off, int len) throws IOException {
		delegate.readFully(b, off, len);
	}

	@Override
	public void readFully(byte[] b) throws IOException {
		delegate.readFully(b);
	}

	@Override
	public int readInt() throws IOException {
		return delegate.readInt();
	}

	@Override
	public String readLine() throws IOException {
		return delegate.readLine();
	}

	@Override
	public long readLong() throws IOException {
		return delegate.readLong();
	}

	@Override
	public short readShort() throws IOException {
		return delegate.readShort();
	}

	@Override
	public String readUTF() throws IOException {
		return delegate.readUTF();
	}

	@Override
	public int readUnsignedByte() throws IOException {
		return delegate.readUnsignedByte();
	}

	@Override
	public int readUnsignedShort() throws IOException {
		return delegate.readUnsignedShort();
	}

	@Override
	public int skipBytes(int n) throws IOException {
		return delegate.skipBytes(n);
	}

	@Override
	public void write(byte[] b, int off, int len) throws IOException {
		delegate.write(b, off, len);
	}

	@Override
	public void write(byte[] b) throws IOException {
		delegate.write(b);
	}

	@Override
	public void write(int b) throws IOException {
		delegate.write(b);
	}

	@Override
	public void writeBoolean(boolean v) throws IOException {
		delegate.writeBoolean(v);
	}

	@Override
	public void writeByte(int v) throws IOException {
		delegate.writeByte(v);
	}

	@Override
	public void writeBytes(String s) throws IOException {
		delegate.writeBytes(s);
	}

	@Override
	public void writeChar(int v) throws IOException {
		delegate.writeChar(v);
	}

	@Override
	public void writeChars(String s) throws IOException {
		delegate.writeChars(s);
	}

	@Override
	public void writeDouble(double v) throws IOException {
		delegate.writeDouble(v);
	}

	@Override
	public void writeFloat(float v) throws IOException {
		delegate.writeFloat(v);
	}

	@Override
	public void writeInt(int v) throws IOException {
		delegate.writeInt(v);
	}

	@Override
	public void writeLong(long v) throws IOException {
		delegate.writeLong(v);
	}

	@Override
	public void writeShort(int v) throws IOException {
		delegate.writeShort(v);
	}

	@Override
	public void writeUTF(String s) throws IOException {
		delegate.writeUTF(s);
	}

}
