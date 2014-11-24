/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class DifferentAccessPaths {
	
	public String foo;

	public void store(){
		Test.deleteAllInstances(this);
		DifferentAccessPaths dap = new DifferentAccessPaths();
		dap.foo = "hi";
		Test.store(dap);
		dap = new DifferentAccessPaths();
		dap.foo = "hi too";
		Test.store(dap);
	}

	public void test(){
		DifferentAccessPaths dap = query();
		for(int i = 0; i < 10; i ++){
			Test.ensure(dap == query());
		}
		Test.objectContainer().purge(dap);
		Test.ensure(dap != query());
	}

	private DifferentAccessPaths query(){
		Query q = Test.query();
		q.constrain(DifferentAccessPaths.class);
		q.descend("foo").constrain("hi");
		ObjectSet os = q.execute();
		DifferentAccessPaths dap = (DifferentAccessPaths)os.next();
		return dap;
	}

}
