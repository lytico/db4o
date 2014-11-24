/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Qlin;

namespace Db4objects.Db4o.Tests.Common.Qlin
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Qlin.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(PrototypesTestCase) };
		}
		// BasicQLinTestCase.class,
	}
}
#endif // !SILVERLIGHT
