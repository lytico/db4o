/* Copyright (C) 2004 - 2012  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 */
public class DalvikSignatureGenerator {
	
	private static final Random _random = new Random();
	
	private static int _counter;
	
	public static String generateSignature() {
		StringBuffer sb = new StringBuffer();
		sb.append(Long.toHexString(System.currentTimeMillis()));
		sb.append(Integer.toHexString(_counter++));
		while(sb.length() < 31){
			sb.append(Integer.toHexString(randomInt()));
		}
		return sb.toString().substring(0, 31);
	}

	private static int randomInt() {
		return _random.nextInt();
	}

}
