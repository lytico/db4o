/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;


/**
 * @exclude
 */
public class SerializedGraph {
	
	public final int _id;

	public final byte[] _bytes;
	
	public SerializedGraph(int id, byte[] bytes) {
		_id = id;
		_bytes = bytes;
	}
	
	public int length(){
		return _bytes.length;
	}
	
	public int marshalledLength(){
		return (Const4.INT_LENGTH * 2 )+ length();
	}
	
	public void write(ByteArrayBuffer buffer){
		buffer.writeInt(_id);
		buffer.writeInt(length());
		buffer.append(_bytes);
	}
	
	public static SerializedGraph read(ByteArrayBuffer buffer){
		int id = buffer.readInt();
		int length = buffer.readInt();
		return new SerializedGraph(id, buffer.readBytes(length));
	}
	
}
