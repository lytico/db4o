/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class ExceptionInTearDownDoesNotShadowTestCase : ITestCase
	{
		public static readonly string InTestMessage = "in test";

		public static readonly string InTeardownMessage = "in teardown";

		public class RunsWithExceptions : ITestLifeCycle
		{
			public virtual void SetUp()
			{
			}

			public virtual void TearDown()
			{
				throw new Exception(InTeardownMessage);
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void TestMethod()
			{
				throw FrameworkTestCase.Exception;
			}
		}

		public class RunsWithExceptionInTearDown : ITestLifeCycle
		{
			public virtual void SetUp()
			{
			}

			public virtual void TearDown()
			{
				throw FrameworkTestCase.Exception;
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void TestMethod()
			{
			}
		}

		public virtual void TestExceptions()
		{
			IEnumerator tests = new ReflectionTestSuiteBuilder(typeof(ExceptionInTearDownDoesNotShadowTestCase.RunsWithExceptions
				)).GetEnumerator();
			ITest test = (ITest)Iterators.Next(tests);
			FrameworkTestCase.RunTestAndExpect(test, 1);
		}

		public virtual void TestExceptionInTearDown()
		{
			IEnumerator tests = new ReflectionTestSuiteBuilder(typeof(ExceptionInTearDownDoesNotShadowTestCase.RunsWithExceptionInTearDown
				)).GetEnumerator();
			ITest test = (ITest)Iterators.Next(tests);
			FrameworkTestCase.RunTestAndExpect(test, 1);
		}
	}
}
