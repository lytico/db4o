/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Mocking;
using Db4oUnit.Tests;

namespace Db4oUnit.Tests
{
	public class CompositeTestListenerTestCase : ITestCase
	{
		internal sealed class ListenerRecorder : ITestListener
		{
			private readonly MethodCallRecorder _recorder;

			private readonly string _label;

			public ListenerRecorder(string label, MethodCallRecorder recorder)
			{
				_label = label;
				_recorder = recorder;
			}

			public void RunFinished()
			{
				Record("runFinished");
			}

			public void RunStarted()
			{
				Record("runStarted");
			}

			public void TestFailed(ITest test, Exception failure)
			{
				Record("testFailed", new object[] { test, failure });
			}

			public void TestStarted(ITest test)
			{
				Record("testStarted", new object[] { test });
			}

			public void Failure(string msg, Exception failure)
			{
				Record("failure", new object[] { msg, failure });
			}

			private void Record(string name)
			{
				Record(name, new object[0]);
			}

			private void Record(string name, object[] args)
			{
				_recorder.Record(new MethodCall(_label + "." + name, args));
			}
		}

		public virtual void Test()
		{
			MethodCallRecorder recorder = new MethodCallRecorder();
			CompositeTestListener listener = new CompositeTestListener(new CompositeTestListenerTestCase.ListenerRecorder
				("first", recorder), new CompositeTestListenerTestCase.ListenerRecorder("second"
				, recorder));
			RunsGreen test = new RunsGreen();
			Exception failure = new Exception();
			listener.RunStarted();
			listener.TestStarted(test);
			listener.TestFailed(test, failure);
			listener.RunFinished();
			recorder.Verify(new MethodCall[] { Call("first.runStarted"), Call("second.runStarted"
				), Call("first.testStarted", test), Call("second.testStarted", test), Call("first.testFailed"
				, test, failure), Call("second.testFailed", test, failure), Call("first.runFinished"
				), Call("second.runFinished") });
		}

		private MethodCall Call(string method, object arg0, Exception arg1)
		{
			return new MethodCall(method, new object[] { arg0, arg1 });
		}

		private MethodCall Call(string method, object arg)
		{
			return new MethodCall(method, new object[] { arg });
		}

		private MethodCall Call(string method)
		{
			return new MethodCall(method, new object[] {  });
		}
	}
}
