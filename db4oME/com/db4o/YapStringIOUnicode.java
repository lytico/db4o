/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public final class YapStringIOUnicode extends YapStringIO{
	
    int bytesPerChar(){
        return 2;
    }
    
	byte encodingByte(){
		return YapConst.UNICODE;
	}
	
//	Currently not needed

//	boolean isEqual(YapBytes bytes, String a_string){
//		char ch;
//		final int len = a_string.length();
//		for (int ii = 0; ii < len; ii ++){
//			ch = a_string.charAt(ii);
//			if(bytes.i_bytes[bytes.i_offset++] != (byte) (ch & 0xff)){
//				return false;
//			}
//			if(bytes.i_bytes[bytes.i_offset++] != (byte)(ch >> 8)){
//				return false;
//			}
//		}
//		return true;
//	}
	
	int length(String a_string){
		return (a_string.length() * 2) + YapConst.OBJECT_LENGTH + YapConst.YAPINT_LENGTH;
	}
	
	public String read(YapReader bytes, int a_length){
	    checkBufferLength(a_length);
		for(int ii = 0; ii < a_length; ii++){
			chars[ii] = (char)((bytes._buffer[bytes._offset ++]& 0xff) | ((bytes._buffer[bytes._offset ++]& 0xff) << 8));
		}
		return new String(chars,0,a_length);
	}
	
	String read(byte[] a_bytes){
	    int len = a_bytes.length / 2;
	    checkBufferLength(len);
	    int j = 0;
	    for(int ii = 0; ii < len; ii++){
	        chars[ii] = (char)((a_bytes[j++]& 0xff) | ((a_bytes[j++]& 0xff) << 8));
	    }
	    return new String(chars,0,len);
	}
	
	int shortLength(String a_string){
		return (a_string.length() * 2)  + YapConst.YAPINT_LENGTH;
	}
	
	void write(YapReader bytes, String string){
	    final int len = writetoBuffer(string);
	    for (int i = 0; i < len; i ++){
			bytes._buffer[bytes._offset++] = (byte) (chars[i] & 0xff);
			bytes._buffer[bytes._offset++] = (byte) (chars[i] >> 8);
		}
	}
	
	byte[] write(String string){
	    final int len = writetoBuffer(string);
	    byte[] bytes = new byte[len * 2];
	    int j = 0;
	    for (int i = 0; i < len; i ++){
	        bytes[j++] = (byte) (chars[i] & 0xff);
	        bytes[j++] = (byte) (chars[i] >> 8);
	    }
	    return bytes;
	}
	
}
