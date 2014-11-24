/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Tests;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4oUnit.Extensions.Tests
{
	public class UnhandledExceptionInThreadTestCase : ITestCase
	{
		public class ExceptionThrowingTestCase : AbstractDb4oTestCase
		{
			public virtual void Test()
			{
				Container().ThreadPool().Start(ReflectPlatform.SimpleName(typeof(UnhandledExceptionInThreadTestCase
					)) + " Throwing Exception Thread", new _IRunnable_15());
			}

			private sealed class _IRunnable_15 : IRunnable
			{
				public _IRunnable_15()
				{
				}

				public void Run()
				{
					throw new InvalidOperationException();
				}
			}
		}

		public virtual void TestSolo()
		{
			Db4oTestSuiteBuilder suite = new Db4oTestSuiteBuilder(new Db4oInMemory(), typeof(
				UnhandledExceptionInThreadTestCase.ExceptionThrowingTestCase));
			TestResult result = new TestResult();
			new TestRunner(suite).Run(result);
			Assert.AreEqual(1, result.Failures.Count);
		}
	}
}
