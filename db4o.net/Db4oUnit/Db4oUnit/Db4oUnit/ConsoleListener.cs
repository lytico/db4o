/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4oUnit;

namespace Db4oUnit
{
	public class ConsoleListener : ITestListener
	{
		private readonly TextWriter _writer;

		public ConsoleListener(TextWriter writer)
		{
			_writer = writer;
		}

		public virtual void RunFinished()
		{
		}

		public virtual void RunStarted()
		{
		}

		public virtual void TestFailed(ITest test, Exception failure)
		{
			PrintFailure(failure);
		}

		public virtual void TestStarted(ITest test)
		{
			Print(test.Label());
		}

		private void PrintFailure(Exception failure)
		{
			if (failure == null)
			{
				Print("\t!");
			}
			else
			{
				Print("\t! " + failure);
			}
		}

		private void Print(string message)
		{
			try
			{
				_writer.Write(message + TestPlatform.NewLine);
				_writer.Flush();
			}
			catch (IOException x)
			{
				TestPlatform.PrintStackTrace(_writer, x);
			}
		}

		public virtual void Failure(string msg, Exception failure)
		{
			Print("\t ! " + msg);
			PrintFailure(failure);
		}
	}
}
