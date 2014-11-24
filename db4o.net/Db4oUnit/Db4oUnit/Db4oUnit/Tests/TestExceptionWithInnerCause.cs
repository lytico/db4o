/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4oUnit.Tests
{
	public class TestExceptionWithInnerCause : ITestCase
	{
		public virtual void TestDetailerMessage()
		{
			string message = "Detailed message";
			TestException e = new TestException(message, new Exception("The reason!"));
			Assert.IsGreaterOrEqual(0, e.ToString().IndexOf(message));
		}
	}
}
