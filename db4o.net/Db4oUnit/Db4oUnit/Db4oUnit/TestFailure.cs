/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4oUnit;

namespace Db4oUnit
{
	public class TestFailure : Printable
	{
		private readonly string _testLabel;

		private readonly Exception _failure;

		public TestFailure(string test, Exception failure)
		{
			_testLabel = test;
			_failure = failure;
		}

		public virtual string TestLabel
		{
			get
			{
				return _testLabel;
			}
		}

		public virtual Exception Reason
		{
			get
			{
				return _failure;
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public override void Print(TextWriter writer)
		{
			writer.Write(_testLabel);
			writer.Write(": ");
			// TODO: don't print the first stack trace elements
			// which reference db4ounit.Assert methods
			TestPlatform.PrintStackTrace(writer, _failure);
		}
	}
}
