/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4oUnit
{
	public interface ITestExecutor
	{
		void Execute(ITest test);

		void Fail(ITest test, Exception exc);
	}
}
