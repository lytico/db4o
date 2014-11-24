/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit
{
	public class TestRunner
	{
		public static DynamicVariable Executor = DynamicVariable.NewInstance();

		private readonly IEnumerable _tests;

		public TestRunner(IEnumerable tests)
		{
			_tests = tests;
		}

		public virtual void Run(ITestListener listener)
		{
			listener.RunStarted();
			ITestExecutor executor = new _ITestExecutor_19(this, listener);
			Environments.RunWith(Environments.NewClosedEnvironment(new object[] { executor })
				, new _IRunnable_28(this, listener));
			listener.RunFinished();
		}

		private sealed class _ITestExecutor_19 : ITestExecutor
		{
			public _ITestExecutor_19(TestRunner _enclosing, ITestListener listener)
			{
				this._enclosing = _enclosing;
				this.listener = listener;
			}

			public void Execute(ITest test)
			{
				this._enclosing.RunTest(test, listener);
			}

			public void Fail(ITest test, Exception failure)
			{
				listener.TestFailed(test, failure);
			}

			private readonly TestRunner _enclosing;

			private readonly ITestListener listener;
		}

		private sealed class _IRunnable_28 : IRunnable
		{
			public _IRunnable_28(TestRunner _enclosing, ITestListener listener)
			{
				this._enclosing = _enclosing;
				this.listener = listener;
			}

			public void Run()
			{
				IEnumerator iterator = this._enclosing._tests.GetEnumerator();
				while (iterator.MoveNext())
				{
					this._enclosing.RunTest((ITest)iterator.Current, listener);
				}
			}

			private readonly TestRunner _enclosing;

			private readonly ITestListener listener;
		}

		private void RunTest(ITest test, ITestListener listener)
		{
			if (test.IsLeafTest())
			{
				listener.TestStarted(test);
			}
			try
			{
				test.Run();
			}
			catch (TestException x)
			{
				Exception reason = x.GetReason();
				listener.TestFailed(test, reason == null ? x : reason);
			}
			catch (Exception failure)
			{
				listener.TestFailed(test, failure);
			}
		}
	}
}
