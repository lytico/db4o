/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.test.*;
import com.db4o.test.legacy.soda.*;
import com.db4o.test.legacy.soda.engines.db4o.*;

public class Soda {
	
	public void test(){
		SodaTest st = new SodaTest(); 
		if(Test.isClientServer()){
		    st.run(SodaTest.CLASSES, new STEngine[] {new STDb4oClientServer()}, true);
		}else{
			st.run(SodaTest.CLASSES, new STEngine[] {new STDb4o()}, true);
		}
		Test.ensure(SodaTest.failedClassesSize() == 0);
		Test.assertionCount += SodaTest.testCaseCount();
	}
}
