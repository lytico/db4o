/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import java.io.*;


public final class PrimitiveCodec {
	
	public static final int INT_LENGTH = 4;
	
	public static final int LONG_LENGTH = 8;
	
	
	public static final int readInt(byte[] buffer, int offset){
        offset += 3;
        return (buffer[offset] & 255) | (buffer[--offset] & 255)
            << 8 | (buffer[--offset] & 255)
            << 16 | buffer[--offset]
            << 24;
	}

	public static final int readInt(ByteArrayInputStream in){
		return (in.read() << 24) | ((in.read() & 255) << 16) | ((in.read() & 255) << 8) | (in.read() & 255);
	}

	public static final void writeInt(byte[] buffer, int offset, int val){
        offset += 3;
        buffer[offset] = (byte)val;
        buffer[--offset] = (byte) (val >>= 8);
        buffer[--offset] = (byte) (val >>= 8);
        buffer[--offset] = (byte) (val >> 8);
	}

	public static final void writeInt(ByteArrayOutputStream out, int val){
		out.write((byte)(val >> 24));
		out.write((byte)(val >> 16));
		out.write((byte)(val >> 8));
        out.write((byte)val);
	}

	public static final void writeLong(byte[] buffer, long val){
		writeLong(buffer, 0, val);
	}
	
	public static final void writeLong(byte[] buffer, int offset, long val){
		for (int i = 0; i < LONG_LENGTH; i++){
			buffer[offset++] = (byte) (val >> ((7 - i) * 8));
		}
	}

	public static final void writeLong(ByteArrayOutputStream out, long val){
		for (int i = 0; i < LONG_LENGTH; i++){
			out.write((byte) (val >> ((7 - i) * 8)));
		}
	}

	public static final long readLong(byte[] buffer, int offset){
		long ret = 0;
		for (int i = 0; i < LONG_LENGTH; i++){
			ret = (ret << 8) + (buffer[offset++] & 0xff);
		}
		return ret;
	}

	public static final long readLong(ByteArrayInputStream in){
		long ret = 0;
		for (int i = 0; i < LONG_LENGTH; i++){
			ret = (ret << 8) + ((byte)in.read() & 0xff);
		}
		return ret;
	}

}
