package com.db4o.util.file;

import java.io.*;
import java.nio.*;

public class RandomAccessByteArray implements RandomAccessBuffer {

	private byte[] buffer;

	private int position = 0;
	private int length = 0;

	public RandomAccessByteArray initializeFrom(DataInput in) throws IOException {
		position = 0;
		length = in.readInt();
		ensureSize(length);
		in.readFully(buffer, 0, length);
		return this;
	}

	private DataInput in = new DataInputStream(new InputStream() {
		@Override
		public int read() throws IOException {
			if (position == length) {
				return -1;
			}
			return buffer[position++];
		}
	});

	private DataOutput out = new DataOutputStream(new OutputStream() {
		@Override
		public void write(int b) throws IOException {
			ensureSize(1);
			buffer[position++] = (byte) (b & 0xff);
			if (position > length) {
				length = position;
			}
		}
	});

	private final int initialCapacity;

	public RandomAccessByteArray(byte[] initialBuffer, int len) {
		buffer = initialBuffer;
		length = len;
		initialCapacity = initialBuffer.length;
	}
	
	public RandomAccessByteArray() {
		this(4);
	}

	public RandomAccessByteArray(int initialCapacity) {
		this.initialCapacity = initialCapacity;
		buffer = new byte[initialCapacity];
	}

	public RandomAccessByteArray(byte[] buffer) {
		this(buffer, buffer.length);
	}

	public RandomAccessByteArray(ByteBuffer buffer) {
		this(buffer.array(), buffer.remaining());
	}

	private void ensureSize(int willGrow) {
		if (buffer.length >= position + willGrow) {
			return;
		}
		int c = buffer.length * 2;
		if (c < position + willGrow) {
			c = position + willGrow + 1;
		}
		byte[] b = new byte[c];
		System.arraycopy(buffer, 0, b, 0, position);
		buffer = b;
	}

	public void write(int b) throws IOException {
		out.write(b);
	}

	public void write(byte[] b) throws IOException {
		out.write(b);
	}

	public void write(byte[] b, int off, int len) throws IOException {
		out.write(b, off, len);
	}

	public void flush() throws IOException {
	}

	public final void writeBoolean(boolean v) throws IOException {
		out.writeBoolean(v);
	}

	public final void writeByte(int v) throws IOException {
		out.writeByte(v);
	}

	public void close() throws IOException {
	}

	public final void writeShort(int v) throws IOException {
		out.writeShort(v);
	}

	public final void writeChar(int v) throws IOException {
		out.writeChar(v);
	}

	public final void writeInt(int v) throws IOException {
		out.writeInt(v);
	}

	public final void writeLong(long v) throws IOException {
		out.writeLong(v);
	}

	public final void writeFloat(float v) throws IOException {
		out.writeFloat(v);
	}

	public final void writeDouble(double v) throws IOException {
		out.writeDouble(v);
	}

	public final void writeBytes(String s) throws IOException {
		out.writeBytes(s);
	}

	public final void writeChars(String s) throws IOException {
		out.writeChars(s);
	}

	public final void writeUTF(String str) throws IOException {
		out.writeUTF(str);
	}

	public final void readFully(byte[] b) throws IOException {
		in.readFully(b);
	}

	public void remove() throws IOException {
		position = 0;
		length = 0;
		buffer = new byte[initialCapacity];
	}

	public final void readFully(byte[] b, int off, int len) throws IOException {
		in.readFully(b, off, len);
	}

	public final int skipBytes(int n) throws IOException {
		return in.skipBytes(n);
	}

	public final boolean readBoolean() throws IOException {
		return in.readBoolean();
	}

	public final byte readByte() throws IOException {
		return in.readByte();
	}

	public final int readUnsignedByte() throws IOException {
		return in.readUnsignedByte();
	}

	public final short readShort() throws IOException {
		return in.readShort();
	}

	public final int readUnsignedShort() throws IOException {
		return in.readUnsignedShort();
	}

	public final char readChar() throws IOException {
		return in.readChar();
	}

	public final int readInt() throws IOException {
		return in.readInt();
	}

	public final long readLong() throws IOException {
		return in.readLong();
	}

	public final float readFloat() throws IOException {
		return in.readFloat();
	}

	public final double readDouble() throws IOException {
		return in.readDouble();
	}

	@Deprecated
	public final String readLine() throws IOException {
		return in.readLine();
	}

	public final String readUTF() throws IOException {
		return in.readUTF();
	}

	@Override
	public int position() throws IOException {
		return position;
	}

	@Override
	public void position(int newPosition) throws IOException {
		this.position = newPosition;
	}

	@Override
	public int length() throws IOException {
		return length;
	}

	@Override
	public boolean isEmpty() {
		try {
			return length() == 0;
		} catch (IOException e) {
			throw new RuntimeException(e);
		}
	}
	
	public byte[] buffer() {
		return buffer;
	}

}
