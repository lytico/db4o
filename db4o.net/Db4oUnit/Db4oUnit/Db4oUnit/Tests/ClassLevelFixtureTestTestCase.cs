/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Tests;

namespace Db4oUnit.Tests
{
	public class ClassLevelFixtureTestTestCase : ITestCase
	{
		private static int _count;

		public virtual void Test()
		{
			_count = 0;
			TestResult result = new TestResult();
			new TestRunner(new ReflectionTestSuiteBuilder(typeof(ClassLevelFixtureTestTestCase.SimpleTestSuite
				))).Run(result);
			Assert.AreEqual(3, _count);
			Assert.AreEqual(1, result.TestCount);
			Assert.AreEqual(0, result.Failures.Count);
		}

		public class SimpleTestSuite : IClassLevelFixtureTest
		{
			public static void ClassSetUp()
			{
				ClassLevelFixtureTestTestCase._count++;
			}

			public static void ClassTearDown()
			{
				ClassLevelFixtureTestTestCase._count++;
			}

			public virtual void Test()
			{
				ClassLevelFixtureTestTestCase._count++;
			}
		}
	}
}
