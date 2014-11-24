package db4ounit.tests.data;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {
	
	public static void main(String[] args) {
	    new AllTests().run();
    }

	protected Class[] testCases() {
		return new Class[] {
			GeneratorsTestCase.class,
			StreamsTestCase.class,
		};
    }
	

}
