/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

/**
 * 
 */
public class SameSizeOnReopen {
	
	public void test(){
		if(! Test.canCheckFileSize()){
			return;
		}
        Db4o.configure().freespace().discardSmallerThan(1);
        Test.close();
        Test.open();
		for (int i = 0; i < 30; i++) {
			Test.close();
			int fileLength = Test.fileLength();
			Test.open();
			Test.commit();
			Test.close();
			int newFileLength = Test.fileLength();
			Test.open();
			Test.ensure(fileLength == newFileLength);
		}
        Db4o.configure().freespace().discardSmallerThan(0);
	}

}
