/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class YapStringIO {
    
    protected char[] chars = new char[0];
    
    int bytesPerChar(){
        return 1;
    }
    
    byte encodingByte(){
		return YapConst.ISO8859;
	}
    
    static YapStringIO forEncoding(byte encodingByte){
        switch (encodingByte) {
        case YapConst.ISO8859:
        	return new YapStringIO();
        default:
            return new YapStringIOUnicode();
        }
    }


// Currently not needed

//	boolean isEqual(YapBytes bytes, String a_string){
//		char ch;
//		final int len = a_string.length();
//		for (int ii = 0; ii < len; ii ++){
//			ch = a_string.charAt(ii);
//			if(bytes.i_bytes[bytes.i_offset++] != (byte) (ch & 0xff)){
//				return false;
//			}
//		}
//		return true;
//	}
	
	int length(String a_string){
		return a_string.length() + YapConst.OBJECT_LENGTH + YapConst.YAPINT_LENGTH;
	}
	
	protected void checkBufferLength(int a_length){
	    if(a_length > chars.length){
	        chars = new char[a_length];
	    }
	}
	
	public String read(YapReader bytes, int a_length){
	    checkBufferLength(a_length);
		for(int ii = 0; ii < a_length; ii++){
			chars[ii] = (char)(bytes._buffer[bytes._offset ++]& 0xff);
		}
		return new String(chars,0,a_length);
	}
	
	String read(byte[] a_bytes){
	    checkBufferLength(a_bytes.length);
	    for(int i = 0; i < a_bytes.length; i++){
	        chars[i] = (char)(a_bytes[i]& 0xff);
	    }
	    return new String(chars,0,a_bytes.length);
	}
	
	int shortLength(String a_string){
		return a_string.length() + YapConst.YAPINT_LENGTH;
	}
	
	protected int writetoBuffer(String str){
	    final int len = str.length();
	    checkBufferLength(len);
	    str.getChars(0, len, chars, 0);
	    return len;
	}
	        
	
	void write(YapReader bytes, String string){
	    final int len = writetoBuffer(string);
	    for (int i = 0; i < len; i ++){
			bytes._buffer[bytes._offset++] = (byte) (chars[i] & 0xff);
		}
	}
	
	byte[] write(String string){
	    final int len = writetoBuffer(string);
	    byte[] bytes = new byte[len];
	    for (int i = 0; i < len; i ++){
	        bytes[i] = (byte) (chars[i] & 0xff);
	    }
	    return bytes;
	}
	
}
