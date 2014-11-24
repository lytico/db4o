/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class VersantDrsTestSuite : DrsTestSuite
	{
		protected override Type[] TestCases()
		{
			return Concat(base.TestCases(), SpecificTestcases());
		}

		private Type[] SpecificTestcases()
		{
			return new Type[] { typeof(ArrayTestSuite), typeof(CustomArrayListTestCase) };
		}
	}
}
