/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class FrameworkTestCase : ITestCase
	{
		public static readonly Exception Exception = new Exception();

		public virtual void TestRunsGreen()
		{
			TestResult result = new TestResult();
			new TestRunner(Iterators.SingletonIterable(new RunsGreen())).Run(result);
			Assert.IsTrue(result.Failures.Count == 0, "not green");
		}

		public virtual void TestRunsRed()
		{
			TestResult result = new TestResult();
			new TestRunner(Iterators.SingletonIterable(new RunsRed(Exception))).Run(result);
			Assert.IsTrue(result.Failures.Count == 1, "not red");
		}

		public static void RunTestAndExpect(ITest test, int expFailures)
		{
			RunTestAndExpect(test, expFailures, true);
		}

		public static void RunTestAndExpect(ITest test, int expFailures, bool checkException
			)
		{
			RunTestAndExpect(Iterators.SingletonIterable(test), expFailures, checkException);
		}

		public static void RunTestAndExpect(IEnumerable tests, int expFailures, bool checkException
			)
		{
			TestResult result = new TestResult();
			new TestRunner(tests).Run(result);
			if (expFailures != result.Failures.Count)
			{
				Assert.Fail(result.Failures.ToString());
			}
			if (checkException)
			{
				for (IEnumerator iter = result.Failures.GetEnumerator(); iter.MoveNext(); )
				{
					TestFailure failure = (TestFailure)iter.Current;
					Assert.AreEqual(Exception, failure.Reason);
				}
			}
		}
	}
}
