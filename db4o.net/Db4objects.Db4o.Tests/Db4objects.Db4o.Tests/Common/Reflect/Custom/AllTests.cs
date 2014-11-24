/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class AllTests : ComposibleTestSuite
	{
		protected override Type[] TestCases()
		{
			return ComposeWith();
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(CustomReflectorTestCase) };
		}
		#endif // !SILVERLIGHT

		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Reflect.Custom.AllTests().RunSolo();
		}
	}
}
