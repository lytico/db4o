/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Mocking;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class TestRunnerTestCase : ITestCase
	{
		internal static readonly Exception FailureException = new Exception();

		public virtual void TestRun()
		{
			RunsGreen greenTest = new RunsGreen();
			RunsRed redTest = new RunsRed(FailureException);
			IEnumerable tests = Iterators.Iterable(new object[] { greenTest, redTest });
			MethodCallRecorder recorder = new MethodCallRecorder();
			ITestListener listener = new _ITestListener_23(recorder);
			new TestRunner(tests).Run(listener);
			recorder.Verify(new MethodCall[] { new MethodCall("runStarted", new object[] {  }
				), new MethodCall("testStarted", new object[] { greenTest }), new MethodCall("testStarted"
				, new object[] { redTest }), new MethodCall("testFailed", new object[] { redTest
				, FailureException }), new MethodCall("runFinished", new object[] {  }) });
		}

		private sealed class _ITestListener_23 : ITestListener
		{
			public _ITestListener_23(MethodCallRecorder recorder)
			{
				this.recorder = recorder;
			}

			public void TestStarted(ITest test)
			{
				recorder.Record(new MethodCall("testStarted", new object[] { test }));
			}

			public void TestFailed(ITest test, Exception failure)
			{
				recorder.Record(new MethodCall("testFailed", new object[] { test, failure }));
			}

			public void RunStarted()
			{
				recorder.Record(new MethodCall("runStarted", new object[] {  }));
			}

			public void RunFinished()
			{
				recorder.Record(new MethodCall("runFinished", new object[] {  }));
			}

			public void Failure(string msg, Exception failure)
			{
				recorder.Record(new MethodCall("failure", new object[] { msg, failure }));
			}

			private readonly MethodCallRecorder recorder;
		}

		public virtual void TestRunWithException()
		{
			ITest test = new _ITest_58();
			//$NON-NLS-1$
			IEnumerable tests = Iterators.Iterable(new object[] { test });
			TestResult result = new TestResult();
			new TestRunner(tests).Run(result);
			Assert.AreEqual(1, result.Failures.Count);
		}

		private sealed class _ITest_58 : ITest
		{
			public _ITest_58()
			{
			}

			public string Label()
			{
				return "Test";
			}

			public void Run()
			{
				Assert.AreEqual(0, 1);
			}

			public bool IsLeafTest()
			{
				return true;
			}

			public ITest Transmogrify(IFunction4 fun)
			{
				return ((ITest)fun.Apply(this));
			}
		}
	}
}
