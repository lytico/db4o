package db4ounit.tests;

import db4ounit.*;

public class ClassLevelFixtureTestTestCase implements TestCase {

	private static int _count;
	
	public void test() {
		_count = 0;
		TestResult result = new TestResult();
		new TestRunner(new ReflectionTestSuiteBuilder(SimpleTestSuite.class)).run(result);
		Assert.areEqual(3, _count);
		Assert.areEqual(1, result.testCount());
		Assert.areEqual(0, result.failures().size());
	}
	
	public static class SimpleTestSuite implements ClassLevelFixtureTest {
		public static void classSetUp() {
			ClassLevelFixtureTestTestCase._count++;
			
		}
		
		public static void classTearDown() {
			ClassLevelFixtureTestTestCase._count++;
		}

		public void test() {
			ClassLevelFixtureTestTestCase._count++;
		}
	}
	
}
