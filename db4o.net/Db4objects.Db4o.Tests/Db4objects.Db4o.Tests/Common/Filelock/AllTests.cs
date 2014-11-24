/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Filelock
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Filelock.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] {  };
		}
	}
}
