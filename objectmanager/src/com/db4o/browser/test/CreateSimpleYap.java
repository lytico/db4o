package com.db4o.browser.test;

import java.io.File;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;

public class CreateSimpleYap {
	private static final String YAPFILENAME = "simple.yap";

	public static void main(String[] args) {
		new File(YAPFILENAME).delete();
		ObjectContainer db=Db4o.openFile(YAPFILENAME);
		TestData prev=null;
		for(int idx=0;idx<10;idx++) {
			TestData current=new TestData(idx,"S"+idx,prev);
			db.set(current);
			prev=current;
		}
		db.close();
	}
}
