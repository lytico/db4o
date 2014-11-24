/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import com.db4o.config.*;
import com.db4o.config.encoding.*;
import com.db4o.internal.encoding.*;

public class CustomStringEncodingTestCase extends StringEncodingTestCaseBase {

	protected void configure(Configuration config) throws Exception {
		config.stringEncoding(new StringEncoding() {
			public byte[] encode(String str) {
				int length = str.length();
			    char[] chars = new char[length];
			    str.getChars(0, length, chars, 0);
			    byte[] bytes = new byte[length * 4];
			    int count = 0;
			    for (int i = 0; i < length; i ++){
			    	bytes[count++] = (byte) (chars[i] & 0xff);
			    	bytes[count++] = (byte) (chars[i] >> 8);
			    	bytes[count++] = (byte)i; // bogus bytes, just for testing
			    	bytes[count++] = (byte)i;
				}
				return bytes;
			}
		
			public String decode(byte[] bytes, int start, int length) {
			    int stringLength =  length / 4;
			    char[] chars = new char[stringLength];
			    int j = start;
			    for(int ii = 0; ii < stringLength; ii++){
			        chars[ii] = (char)((bytes[j++]& 0xff) | ((bytes[j++]& 0xff) << 8));
			        j+=2;
			    }
			    return new String(chars,0,stringLength);
			}
		
		});
	}
	
	protected Class stringIoClass() {
		return DelegatingStringIO.class;
	}

}
