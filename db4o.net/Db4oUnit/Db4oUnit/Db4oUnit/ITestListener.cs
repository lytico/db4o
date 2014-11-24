/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4oUnit
{
	public interface ITestListener
	{
		void RunStarted();

		void TestStarted(ITest test);

		void TestFailed(ITest test, Exception failure);

		void RunFinished();

		void Failure(string msg, Exception failure);
	}
}
