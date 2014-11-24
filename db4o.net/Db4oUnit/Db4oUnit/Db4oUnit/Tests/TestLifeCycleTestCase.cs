/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit.Tests
{
	public class TestLifeCycleTestCase : ITestCase
	{
		public virtual void TestLifeCycle()
		{
			ByRef tearDownCalled = ByRef.NewInstance(false);
			RunsLifeCycle._tearDownCalled.With(tearDownCalled, new _IRunnable_11());
			Assert.IsTrue((((bool)tearDownCalled.value)));
		}

		private sealed class _IRunnable_11 : IRunnable
		{
			public _IRunnable_11()
			{
			}

			public void Run()
			{
				IEnumerator tests = new ReflectionTestSuiteBuilder(typeof(RunsLifeCycle)).GetEnumerator
					();
				ITest test = (ITest)Iterators.Next(tests);
				FrameworkTestCase.RunTestAndExpect(test, 1);
			}
		}
	}
}
