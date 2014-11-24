/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Exceptions.Propagation.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ExceptionDuringTopLevelCallTestSuite) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(Db4objects.Db4o.Tests.Common.Exceptions.Propagation.CS.AllTests
				) };
		}
		#endif // !SILVERLIGHT
	}
}
