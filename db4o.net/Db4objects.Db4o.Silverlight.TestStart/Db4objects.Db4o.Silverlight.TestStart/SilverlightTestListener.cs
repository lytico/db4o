/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Browser;
using System.Windows.Threading;
using Db4oUnit;

namespace Db4objects.Db4o.Silverlight.TestStart
{
	public class SilverlightTestListener : ITestListener
	{
		public SilverlightTestListener(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public void RunStarted()
		{
			Run("append", "Tests started...");
			_startTime = DateTime.Now;
		}

		public void TestStarted(ITest test)
		{
			NewTest(test.Label());
		}

		public void TestFailed(ITest test, Exception failure)
		{
			_failures.Add(new TestFailure(test.Label(), failure));
			MarkLastAsError();
		}
		
		public void Failure(String msg, Exception failure)
		{
			_failures.Add(new TestFailure(msg, failure));
		}

		public void RunFinished()
		{
			AppendTestsResult();
			Append(FailuresMessage());
		}

		private void Append(string message)
		{
			Run("append", message);
		}

		private void AppendTestsResult()
		{
			Run("appendTestsResult", DateTime.Now.Subtract(_startTime).TotalSeconds);
		}

		private void NewTest(string message)
		{
			_latestAppended = Run("newTest", message);
		}

		private static string RemoveExtraCommaAtStart(string arguments)
		{
			return arguments.Remove(0, 1);
		}

		private void Dispatch(Action action)
		{
			_dispatcher.BeginInvoke(action);
		}

		private void MarkLastAsError()
		{
			_dispatcher.BeginInvoke(() => HtmlPage.Window.Invoke("markAsFailure", _latestAppended));
		}

		private string FailuresMessage()
		{
			int count = 1;
			return
				_failures.Aggregate(new StringBuilder(),
									(acc, failure) =>
									acc.AppendFormat("{0}) {1} {2}<br /><br />", count++, failure.TestLabel, failure.Reason)).Replace("\r\n", "<br />").ToJScriptString();
		}

		private object Run(string functionName, params object[] args)
		{
			Dispatch(delegate
			{
			    string code = functionName + "(" + ToStringArgumentList(args) + ");";
			    object result = HtmlPage.Window.Eval(code);
				if (result != null)
				{
					_latestAppended = result;
				}
			});
			return _latestAppended;
		}

		private static string ToStringArgumentList(IEnumerable<object> objects)
		{
			string arguments = objects.Aggregate("", (acc, current) => acc + "," + AddQuotes(RemoveInternalQuotes(current.ToJScriptString())));
			return RemoveExtraCommaAtStart(arguments);
		}

	    private static string RemoveInternalQuotes(string source)
	    {
	        return source.Replace('"', '\'');
	    }

		private static string AddQuotes(object item)
		{
			return "\"" + item + "\"";
		}

		private object _latestAppended;
		private DateTime _startTime;
		private readonly IList<TestFailure> _failures = new List<TestFailure>();
		private readonly Dispatcher _dispatcher;
		
	}
}
