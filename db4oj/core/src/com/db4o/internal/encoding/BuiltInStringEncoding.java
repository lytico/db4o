/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.encoding;

import com.db4o.config.encoding.*;

/**
 * @exclude
 */
public abstract class BuiltInStringEncoding implements StringEncoding {
	
	/**
	 * keep the position in the array. 
	 * Information is used to look up encodings.  
	 */
	private static final BuiltInStringEncoding[] ALL_ENCODINGS = new BuiltInStringEncoding[] {
		null,
		new LatinStringEncoding(),
		new UnicodeStringEncoding(),
		new UTF8StringEncoding(), 
	};
	
	public static byte encodingByteForEncoding(StringEncoding encoding){
		for (int i = 1; i < ALL_ENCODINGS.length; i++) {
			if(encoding.getClass() == ALL_ENCODINGS[i].getClass()){
				return (byte) i;
			}
		}
		return 0;
	}
	
    public static LatinStringIO stringIoForEncoding(byte encodingByte, StringEncoding encoding){
    	if(encodingByte < 0 || encodingByte > ALL_ENCODINGS.length){
    		throw new IllegalArgumentException();
    	}
		if(encodingByte == 0){
			if(encoding instanceof BuiltInStringEncoding){
				System.out.println("Warning! Database was created with a custom string encoding but no custom string encoding is configured for this session.");
			}
			return new DelegatingStringIO(encoding);
		};
		BuiltInStringEncoding builtInEncoding = ALL_ENCODINGS[encodingByte];
		return builtInEncoding.createStringIo(encoding);
    }

	protected LatinStringIO createStringIo(StringEncoding encoding) {
		return new DelegatingStringIO(encoding);
	}


}
