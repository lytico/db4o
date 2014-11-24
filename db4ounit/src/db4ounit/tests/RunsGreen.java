/**
 * 
 */
package db4ounit.tests;

import com.db4o.foundation.*;

import db4ounit.*;

class RunsGreen implements Test {
	
	public String label() {
		return "RunsGreen";
	}

	public void run() {
	}

	public boolean isLeafTest() {
		return true;
	}
	
	public Test transmogrify(Function4<Test, Test> fun) {
		return fun.apply(this);
	}
}