/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.encoding;

import com.db4o.internal.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public final class UnicodeStringIO extends LatinStringIO{
	
    protected int bytesPerChar(){
        return 2;
    }
    
    public byte encodingByte(){
    	return BuiltInStringEncoding.encodingByteForEncoding(new UnicodeStringEncoding());
	}
	
	public int length(String str){
		return (str.length() * 2) + Const4.OBJECT_LENGTH + Const4.INT_LENGTH;
	}
	
	public String read(ReadBuffer buffer, int length){
	    char[] chars = new char[length];
		for(int ii = 0; ii < length; ii++){
			chars[ii] = (char)((buffer.readByte() & 0xff) | ((buffer.readByte() & 0xff) << 8));
		}
		return new String(chars, 0, length);
	}
	
	public String read(byte[] bytes){
	    int length = bytes.length / 2;
	    char[] chars = new char[length];
	    int j = 0;
	    for(int ii = 0; ii < length; ii++){
	        chars[ii] = (char)((bytes[j++]& 0xff) | ((bytes[j++]& 0xff) << 8));
	    }
	    return new String(chars,0,length);
	}
	
	public int shortLength(String str){
		return (str.length() * 2)  + Const4.INT_LENGTH;
	}
	
	public void write(WriteBuffer buffer, String str){
	    final int length = str.length();
	    char[] chars = new char[length];
	    str.getChars(0, length, chars, 0);
	    for (int i = 0; i < length; i ++){
	        buffer.writeByte((byte) (chars[i] & 0xff));
	        buffer.writeByte((byte) (chars[i] >> 8));
		}
	}
	
	public byte[] write(String str){
	    final int length = str.length();
	    char[] chars = new char[length];
	    str.getChars(0, length, chars, 0);
	    byte[] bytes = new byte[length * 2];
	    int j = 0;
	    for (int i = 0; i < length; i ++){
	        bytes[j++] = (byte) (chars[i] & 0xff);
	        bytes[j++] = (byte) (chars[i] >> 8);
	    }
	    return bytes;
	}
	
}
