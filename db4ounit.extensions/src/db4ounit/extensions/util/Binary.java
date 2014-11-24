/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.util;

/**
 * @exclude
 */
public class Binary {
	
	public static long longForBits(long bits){
		return (long) ((Math.pow(2, bits)) - 1);
	}
	
	public static int numberOfBits(long l){
		if(l < 0){
			throw new IllegalArgumentException();
		}
		long bit = 1;
		int counter = 0;
		for (int i = 0; i < 64; i++) {
			if( (l & bit) == 0){
				counter ++;
			} else{
				counter = 0;
			}
			bit = bit << 1;
		}
		return 64 - counter;
	}


}
