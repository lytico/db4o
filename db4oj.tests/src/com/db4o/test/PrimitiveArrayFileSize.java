/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class PrimitiveArrayFileSize {
	
	Object arr;
	
	public void testSimpleLongInObject(){
		int call = 0;
		PrimitiveArrayFileSize pafs = new PrimitiveArrayFileSize();
		for (int i = 0; i < 12; i++) {
			pafs.arr = new long[100];
			Test.store(pafs);
			checkFileSize(call++);
			Test.commit();
			checkFileSize(call++);
		}
	}
	
	public void testLongWrapperInObject(){
		int call = 0;
		PrimitiveArrayFileSize pafs = new PrimitiveArrayFileSize();
		for (int i = 0; i < 12; i++) {
			pafs.arr = longWrapperArray();
			Test.store(pafs);
			checkFileSize(call++);
			Test.commit();
			checkFileSize(call++);
		}
	}
	
	public void testSimpleLongInHashMap(){
		HashMap hm = new HashMap();
		int call = 0;
		for (int i = 0; i < 12; i++) {
			long[] lll = new long[100];
			lll[0] = 99999;
			hm.put("test", lll);
			Test.store(hm);
			checkFileSize(call++);
			Test.commit();
			checkFileSize(call++);
		}
	}
	
	public void testLongWrapperInHashMap(){
		HashMap hm = new HashMap();
		int call = 0;
		for (int i = 0; i < 12; i++) {
			hm.put("test", longWrapperArray());
			Test.store(hm);
			checkFileSize(call++);
			Test.commit();
			checkFileSize(call++);
		}
	}
	
	
	private Long[] longWrapperArray(){
		Long[] larr = new Long[100];
		for (int j = 0; j < larr.length; j++) {
			larr[j] = new Long(j);
		}
		return larr;
	}
	
	
	
	private void checkFileSize(int call){
		if(Test.canCheckFileSize()){
			int newFileLength = Test.fileLength();
			
			// Interesting for manual tests:
			// System.out.println(newFileLength);
			
			if(call == 6){
				// consistency reached, start testing
				jumps = 0;
				fileLength = newFileLength;
			}else if(call > 6){
				if(newFileLength > fileLength){
					if(jumps < 4){
						fileLength = newFileLength;
						jumps ++;
						// allow two further step in size
						// may be necessary for commit space extension
					}else{
						// now we want constant behaviour
						Test.error();
					}
				}
			}
		}
	}
	
	private static transient int fileLength;
	private static transient int jumps; 
	
	
}

