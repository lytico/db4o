/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Sessions;

namespace Db4objects.Db4o.Tests.Common.Sessions
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(EmbeddedObjectContainerSessionTestCase) };
		}

		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Sessions.AllTests().RunSolo();
		}
	}
}
