/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Acid;

namespace Db4objects.Db4o.Tests.Common.Acid
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Acid.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeWith();
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(ReadCommittedIsolationTestCase) };
		}
		#endif // !SILVERLIGHT
		//				CrashSimulatingTestSuite.class,
	}
}
