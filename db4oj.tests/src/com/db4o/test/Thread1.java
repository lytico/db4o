/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.test.types.*;

public class Thread1 extends Regression implements Runnable
{
	public Thread1(Regression openDelegate){
		this.openDelegate = openDelegate;
	}
	
	public void run(){
		Thread.currentThread().setName("Thread1");
		run1(testClasses());
	}

	public RTestable[] testClasses () {
		return  new RTestable[] {
            new ObjectSimplePublic()
			};
	}
	
	protected boolean closeFile(){
		return false;
	}
	
	protected int runs(){
		return THREAD_RUNS;
	}
}
